---
id: bug-100
title: weekly-focus-tile.component.scss var() calls omit the design-system fallback hex
status: Fixed
---

# Bug 100 — weekly-focus var() fallback hex

**Status**: Fixed

## Fix

Added the design-system hex fallback to three `var()` references
in `weekly-focus-tile.component.scss`:

- `var(--cui-text-primary, #FFFFFF)` (focus name `<strong>`)
- `var(--cui-text-secondary, #B0B0B0)` (supporting metric, .empty)

Same shape as bug-096/097/098/099. The bug-100 spec adds a
forward-only regex guard. 297/297 workspace tests green.

## Description

Same shape as bug-096/097/098/099: three `var(--cui-*)`
references in `weekly-focus-tile.component.scss` are missing
the cui-ui-style hex fallback:

```scss
.focus-list strong { color: var(--cui-text-primary); }
.focus-list span   { color: var(--cui-text-secondary); }
.empty             { color: var(--cui-text-secondary); }
```

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss
```

Returns three matches.

## Expected

- `var(--cui-text-primary, #FFFFFF)` (focus name)
- `var(--cui-text-secondary, #B0B0B0)` (supporting metric, empty state)

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing weekly-focus specs continue to pass.
