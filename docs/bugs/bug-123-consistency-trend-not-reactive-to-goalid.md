---
id: bug-123
title: Consistency Trend tile doesn't reload when goalId or windowDays input changes after init
status: Fixed
---

# Bug 123 — non-reactive goalId / windowDays

**Status**: Fixed

## Fix

Replaced the `ngOnInit`-based load with an `effect()` in the
constructor that reads both `goalId()` and `windowDays()`. Any
parent input change now reloads the trend, keeping the chart
and the bug-122 dynamic eyebrow in sync. `ngOnInit` is no
longer implemented; the `OnInit` import and interface dropped.
Same shape as goal-metrics bug-072.

320/320 workspace tests green.

## Description

After bug-071 migrated `@Input` to `input()` signals on the
consistency-trend tile, the load logic stayed in `ngOnInit`:

```ts
ngOnInit(): void {
  const id = this.goalId();
  if (id) {
    this.controller.load(id, this.windowDays());
  }
}
```

`ngOnInit` runs once — if a parent later changes the bound
`goalId` or `windowDays`, the controller never reloads. The
chart silently sticks to the original goal/window.

bug-072 already established the modern pattern on
goal-metrics-tile: an `effect()` in the constructor that runs
on every change. Same shape applies here.

After bug-122, the eyebrow already binds to `windowDays()` —
without a reactive load, the eyebrow text and the actual data
window can diverge if anyone overrides `windowDays`.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "ngOnInit\|controller.load" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

The load runs in `ngOnInit`, not in an `effect()`.

## Expected

The init load moves into an `effect()` in the constructor:

```ts
effect(() => {
  const id = this.goalId();
  if (id) {
    this.controller.load(id, this.windowDays());
  }
});
```

`ngOnInit` is removed (or empty). `OnInit` import dropped.

## Verification

- Unit (TS source): assert the controller load runs inside an
  `effect(...)` referencing both `goalId()` and `windowDays()`,
  and that `ngOnInit` no longer carries the `controller.load`
  call.
- All existing consistency-trend specs continue to pass.
