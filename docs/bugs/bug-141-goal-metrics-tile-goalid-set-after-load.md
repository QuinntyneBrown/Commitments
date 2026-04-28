---
id: bug-141
title: goal-metrics-tile setGoalId effect fires AFTER bindTileMode's load — initial load bails on empty _goalId
status: Fixed
---

# Bug 141 — goal-metrics-tile: `_goalId` empty during initial `load()` call

**Status**: Fixed

## Fix

Added a synchronous `controller.setGoalId(this.goalId())` call
as the first line of the constructor, before `bindTileMode`:

```ts
constructor() {
  this.controller.setGoalId(this.goalId());

  const context = inject(TILE_CONTEXT, { optional: true });
  bindTileMode({
    context,
    load: (mode, asOf) => this.controller.load(mode, asOf)
  });
  effect(() => {
    this.controller.setGoalId(this.goalId());
  });
}
```

The synchronous initial set ensures `_goalId` is populated when
`bindTileMode` runs `load('live', null)` synchronously in
standalone mode, and also before the registered effect runs in
context mode. The retained `effect(...)` continues to handle
runtime updates to the `goalId` input signal.

351/351 workspace tests green; the new `bug-141` behavior test
now passes (`getCurrent('demo-goal')` is called on mount).

## Description

In `goal-metrics-tile.component.ts`, the constructor wires:

```ts
constructor() {
  const context = inject(TILE_CONTEXT, { optional: true });
  bindTileMode({
    context,
    load: (mode, asOf) => this.controller.load(mode, asOf)
  });
  effect(() => {
    this.controller.setGoalId(this.goalId());
  });
}
```

In standalone mode (no `TILE_CONTEXT` provided),
`bindTileMode` does this **synchronously** during the
constructor:

```ts
if (!context) {
  load('live', null);
  return;
}
```

…which calls `controller.load('live', null)` immediately. At
that point `controller._goalId` is still the empty string (its
initial declaration value). The first guard in `load()` is:

```ts
load(mode, asOf): void {
  this.mode.set(mode);
  if (!this._goalId) return;
  ...
}
```

So `load` returns early. **`getCurrent()` is never called**, the
tile shows `0 / 0` `0%` indefinitely.

The `effect(() => setGoalId(...))` registered after
`bindTileMode` only fires on the next tick — too late to rescue
the synchronous `load('live', null)` call. There is no callback
that re-invokes `load` once `_goalId` is set, so the initial
fetch is permanently lost in standalone mode.

In context-bound mode (the dashboard host provides
`TILE_CONTEXT`) the bug technically still applies — Angular's
effect scheduler runs effects in registration order on the same
tick, so `bindTileMode`'s reload effect fires first (with empty
`_goalId`) before the `setGoalId` effect runs.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.spec.ts` (add behavior test)

## Reproduction

Mount `<commitments-goal-metrics-tile>` with no `TILE_CONTEXT`
and a mocked `GoalProgressService.getCurrent`. Assert that
`getCurrent` was called with the default goalId `'demo-goal'`.
Currently it is **not** called — `_goalId` is empty when `load`
runs, so the call is skipped.

## Expected

Set the controller's goalId synchronously **before**
`bindTileMode` runs, so the first `load()` call (whether
synchronous in standalone mode or scheduled in context mode)
sees a non-empty `_goalId` and actually issues the HTTP request:

```ts
constructor() {
  this.controller.setGoalId(this.goalId()); // synchronous, before any load

  const context = inject(TILE_CONTEXT, { optional: true });
  bindTileMode({
    context,
    load: (mode, asOf) => this.controller.load(mode, asOf)
  });

  effect(() => {
    this.controller.setGoalId(this.goalId());
  });
}
```

The synchronous initial set ensures `_goalId` is populated
before any `load()` runs. The effect remains for runtime
changes to the `goalId` input signal.

## Verification

- New unit test mounts the standalone tile and asserts
  `goalProgressService.getCurrent` is called with `'demo-goal'`.
- All existing goal-metrics specs continue to pass.
