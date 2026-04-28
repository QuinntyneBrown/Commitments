---
id: bug-102
title: goal-metrics-tile.component.scss var() calls omit the design-system fallback hex
status: Fixed
---

# Bug 102 — goal-metrics var() fallback hex

**Status**: Fixed

## Fix

Added the design-system hex fallback to six `var()` references
in `goal-metrics-tile.component.scss`:

- `var(--cui-text-primary, #FFFFFF)` (count)
- `var(--cui-text-secondary, #B0B0B0)` (separator/target/percent/delta default)
- `var(--cui-success, #66BB6A)` (delta--success)
- `var(--cui-warning, #FFA726)` (delta--warn)

Final tile in the bug-096 series. **Every dashboard-plugin tile
SCSS now follows the cui-ui fallback convention** — daily-results,
outstanding-todos, monthly-progress, relations, weekly-focus,
consistency-trend, and goal-metrics. Each tile spec has a
forward-only regex guard. 299/299 workspace tests green.

## Description

Final tile in the bug-096 series.
`goal-metrics-tile.component.scss` has six `var(--cui-*)`
references missing the cui-ui-style hex fallback:

```scss
.goal-readout__count             { color: var(--cui-text-primary); }
.goal-readout__separator,
.goal-readout__target            { color: var(--cui-text-secondary); }
.goal-percent                    { color: var(--cui-text-secondary); }
.goal-delta                      { color: var(--cui-text-secondary); }
.goal-delta--success             { color: var(--cui-success); }
.goal-delta--warn                { color: var(--cui-warning); }
```

Goal-metrics doesn't have a `/docs/tiles/` design folder, but
the cui-ui fallback convention is code-wide — every other tile
SCSS file already follows it (bug-096–101).

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.scss
```

Returns six matches.

## Expected

- `var(--cui-text-primary, #FFFFFF)` (count)
- `var(--cui-text-secondary, #B0B0B0)` (separator, target, percent, delta default)
- `var(--cui-success, #66BB6A)` (delta--success)
- `var(--cui-warning, #FFA726)` (delta--warn)

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing goal-metrics specs continue to pass.
