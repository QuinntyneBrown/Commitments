---
id: bug-071
title: Consistency-trend uses legacy `@Input()` decorators; commitments-ui primitives all use modern `input()` signals
status: Open
---

# Bug 071 — Consistency-trend `@Input()` migration to `input()` signals

**Status**: Open

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
