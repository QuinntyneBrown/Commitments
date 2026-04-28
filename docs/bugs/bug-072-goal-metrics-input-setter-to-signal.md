---
id: bug-072
title: Goal-metrics uses an `@Input set goalId(...)` decorator with side effects; migrate to `input()` signal + effect
status: Fixed
---

# Bug 072 — Goal-metrics @Input setter migration

**Status**: Fixed

## Fix

`goal-metrics-tile.component.ts`:
- `@Input set goalId(...)` removed.
- New `readonly goalId = input('demo-goal');`.
- Constructor's `controller.setGoalId('demo-goal');` removed.
- New `effect(() => this.controller.setGoalId(this.goalId()));`
  inside the constructor.
- Import group: `Input` dropped; `effect` and `input` added.

The effect runs once on creation (default seeds `'demo-goal'`)
and again whenever the input changes — replacing both the
constructor seed and the setter callback.

`@Input` is now eliminated from every dashboard-plugin tile
component. Each tile uses Angular's modern signal idiom
(`input()` / `computed()` / `effect()`) uniformly.

Coverage:
- New TS-source spec asserts `@Input` is gone, declares `goalId
  = input(`, and the constructor includes an `effect(...)` that
  calls `setGoalId(this.goalId())`.
- All 22 affected suites pass (162/162 — was 161/161 before).

## Description

`goal-metrics-tile.component.ts` declares a setter-style legacy
input:

```ts
@Input() set goalId(value: string) {
  this.controller.setGoalId(value);
}

constructor() {
  this.controller.setGoalId('demo-goal');
  …
}
```

The constructor seeds the controller with `'demo-goal'`; the
setter then forwards any subsequent input changes to the same
controller method.

Bug-071 migrated `consistency-trend`'s simple `@Input` decorators
to `input()` signals. Goal-metrics' setter pattern needs an
`effect()` to replicate the side-effect behavior:

```ts
readonly goalId = input('demo-goal');

constructor() {
  const context = inject(TILE_CONTEXT, …);
  bindTileMode(…);

  effect(() => {
    this.controller.setGoalId(this.goalId());
  });
}
```

The effect runs once on component creation (calling
`setGoalId('demo-goal')` from the input default) and again
whenever the input value changes — replacing both the initial
constructor call and the setter callback.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.ts`

## Reproduction

```bash
grep -n '@Input' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.ts
```

One match — the setter form.

## Expected

- `Input` import dropped; `input` and `effect` added to the
  existing `@angular/core` import.
- `@Input set goalId(...)` removed.
- New `readonly goalId = input('demo-goal');` declaration.
- Constructor's `controller.setGoalId('demo-goal');` removed.
- New `effect(() => this.controller.setGoalId(this.goalId()));`
  inside the constructor.

## Verification

- Unit: source-level TS spec — file no longer references `@Input`,
  declares `goalId = input(`, and the constructor body contains an
  `effect(...)` calling `setGoalId(this.goalId())`.
- Goal-metrics' existing controller spec continues to pass — the
  setter behavior is identical for the default value and for any
  new value bound through the input.
