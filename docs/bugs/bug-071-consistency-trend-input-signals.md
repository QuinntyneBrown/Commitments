---
id: bug-071
title: Consistency-trend uses legacy `@Input()` decorators; commitments-ui primitives all use modern `input()` signals
status: Fixed
---

# Bug 071 — Consistency-trend `@Input()` migration to `input()` signals

**Status**: Fixed

## Fix

`consistency-trend-tile.component.ts`:
- `@Input() goalId = 'demo-goal';` → `readonly goalId = input('demo-goal');`
- `@Input() windowDays = 30;` → `readonly windowDays = input(30);`
- `Input` import dropped; `input` added to the existing
  `@angular/core` import.
- Two consumers in `ngOnInit` updated to invoke as functions
  (`this.goalId()`, `this.windowDays()`).

Bug-061 had already migrated pill members to computed signals;
this finishes the modern-signal alignment for the file.

Coverage:
- New TS-source spec asserts the file no longer references
  `@Input` and declares `goalId = input(`/`windowDays = input(`.
- All 22 affected suites pass (161/161 — was 160/160 before).

Goal-metrics-tile's `@Input set goalId` setter pattern is left
as a separate follow-up — converting it requires wrapping the
controller side-effect in an `effect()`.

## Description

`consistency-trend-tile.component.ts` declares two inputs via the
legacy decorator API:

```ts
@Input() goalId = 'demo-goal';
@Input() windowDays = 30;
```

Every commitments-ui primitive (tile-shell, status-pill,
metric-header, delta-badge, etc.) uses Angular's modern signal
input API:

```ts
readonly title = input('');
readonly variant = input<TileShellVariant>('metric');
```

Bug-061 already converted consistency-trend's pill members to
computed signals for the same cross-tile uniformity reason. The
inputs are the last legacy-decorator vestige in the file; both
are simple-value inputs without setters, so the migration is
mechanical.

The two consumers (`ngOnInit` reading `this.goalId` /
`this.windowDays`) become signal calls (`this.goalId()` /
`this.windowDays()`).

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`

## Reproduction

```bash
grep -n '@Input' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

Two matches.

## Expected

```ts
readonly goalId = input('demo-goal');
readonly windowDays = input(30);

ngOnInit(): void {
  const id = this.goalId();
  if (id) {
    this.controller.load(id, this.windowDays());
  }
}
```

`Input` import dropped (unused after migration); `input` joins the
existing `@angular/core` import group.

## Verification

- Unit: source-level TS spec — file no longer matches `@Input`,
  declares `goalId = input(` and `windowDays = input(`.
- All affected suites continue to pass (no behaviour change for
  the default `'demo-goal'` / `30` values).
