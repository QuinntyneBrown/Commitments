---
id: bug-130
title: GoalMetricsController._reviewDelta() is a method called from two computed signals; should itself be computed
status: Open
---

# Bug 130 — _reviewDelta method → computed signal

**Status**: Open

## Description

`goal-metrics.controller.ts` declares `_reviewDelta()` as a
private method:

```ts
private _reviewDelta(): number | null {
  if (this.mode() !== 'review') return null;
  const c = this.current();
  const today = this.today();
  if (!c || !today) return null;
  return c.count - today.count;
}
```

It's called from two `computed()` signals — `deltaLabel` and
`deltaTone`. Each computed dependency tracks `mode/current/today`
twice (once via the inline call) and re-runs `_reviewDelta`'s
body on each evaluation. Migrating `_reviewDelta` to a
`computed` signal memoizes the result so both `deltaLabel` and
`deltaTone` share a single cached evaluation per
mode/current/today change.

The same controller already uses `computed()` extensively (count,
target, percentage, deltaLabel, deltaTone). Same shape as bug-129
which migrated delta-badge methods.

The outstanding-todos controller has the same pattern — leave
that for a sibling follow-up.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics.controller.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics.controller.spec.ts` (or new spec / source-level)

## Reproduction

```bash
grep -n '_reviewDelta' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics.controller.ts
```

Method declaration at one site; two callers from inside
`computed(() => …)`.

## Expected

`_reviewDelta` is declared as `computed<number | null>(...)`
and read via `this._reviewDelta()` in deltaLabel/deltaTone.

## Verification

- Unit (TS source): assert `_reviewDelta` is declared as
  `= computed(` (no longer method form).
- Existing goal-metrics specs (count, target, percentage, delta)
  keep passing — call sites unchanged.
