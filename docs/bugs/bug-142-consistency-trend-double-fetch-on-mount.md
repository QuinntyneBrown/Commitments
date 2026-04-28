---
id: bug-142
title: consistency-trend tile fires two fetches on initial mount when TILE_CONTEXT is provided
status: Open
---

# Bug 142 — consistency-trend: duplicate `_fetch()` on initial mount

## Description

`ConsistencyTrendTileComponent` registers two separate effects
that each kick off a controller fetch on the first tick when
`TILE_CONTEXT` is provided (the dashboard host case):

```ts
constructor() {
  effect(() => {
    const id = this.goalId();
    if (id) {
      this.controller.load(id, this.windowDays());      // → _fetch() #1
    }
  });

  // ... unrelated chart-update effect ...

  if (this._tileContext) {
    effect(() => {
      this._tileContext!.mode();
      this._tileContext!.selectedReviewDate();
      this.controller.refresh();                          // → _fetch() #2
    });
  }
}
```

Angular's effect scheduler runs same-tick effects in
registration order. On mount:

1. effect-1 reads `goalId` and calls `controller.load('demo-goal', 30)`,
   which sets `_goalId`/`_windowDays` and runs `_fetch()`.
2. effect-3 reads `mode()` and `selectedReviewDate()`, then calls
   `controller.refresh()`, which sees `_goalId` is now set and
   runs `_fetch()` again with the *same* parameters.

For `demo-goal` this means `synthesizeDemoTrend(...)` runs twice
and `trend.set(...)` fires twice — the chart re-renders twice on
mount. For a real backend, two parallel `GET /goal-progress/trend`
requests fire with identical params; the later response wins
(possibly out of order if they take different times) and the
earlier work is wasted.

In standalone mode (no `TILE_CONTEXT`), effect-3 never runs, so
the bug only manifests inside the dashboard host.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts` (add behavior test)

## Reproduction

Mount the tile inside a host that provides a `TILE_CONTEXT` and
mock `GoalTrendService.getTrend`. Pass a non-demo goalId so the
real fetch path runs (demo-goal short-circuits to
`synthesizeDemoTrend`). Assert `getTrend` was called **once**.
Currently it is called twice.

## Expected

Consolidate the two fetch-triggering effects into a single
effect that depends on `goalId`, `windowDays`, and (when context
is present) `mode`/`selectedReviewDate`. The `_fetch()` helper
inside the controller already reads mode/asOf from the same
`Signal<...>` references that the dashboard binds, so reading
them inside the tile-level effect is enough to track changes.

```ts
constructor() {
  effect(() => {
    const id = this.goalId();
    if (!id) return;
    // Track mode/asOf when the tile is hosted by the dashboard:
    this._tileContext?.mode();
    this._tileContext?.selectedReviewDate();
    this.controller.load(id, this.windowDays());
  });
  // ... chart-update effect remains unchanged ...
}
```

The `controller.refresh()` API can stay (it's still useful as a
small public surface), but the tile itself no longer calls it
during normal lifecycle — the single effect handles every
re-fetch trigger in one place.

## Verification

- New unit test mounts the tile with a stubbed `TILE_CONTEXT`
  and a non-demo goalId, asserts `getTrend` is called once.
- All existing consistency-trend specs continue to pass.
