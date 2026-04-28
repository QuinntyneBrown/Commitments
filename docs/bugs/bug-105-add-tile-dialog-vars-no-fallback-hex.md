---
id: bug-105
title: add-tile-dialog.component.scss var() calls omit the design-system fallback hex
status: Fixed
---

# Bug 105 — add-tile-dialog var() fallback hex

**Status**: Fixed

## Fix

Added the design-system hex fallback to all 16 `var(--cui-*)`
references in `add-tile-dialog.component.scss` across surface,
text, divider, accent, surface-2, primary-dim, and primary
tokens. Created a minimal CSS-source spec (no spec previously
existed) with the bug-105 regex guard.

The dialog renders one cell per registered tile from
`tileMetadata.displayName` + icon, so its chrome staying in
sync with the design system is directly tile-related.

302/302 workspace tests green.

## Description

Continuing the bug-096–104 series.
`dashboard-framework`'s
`add-tile-dialog.component.scss` (the modal that lists every
plugin tile so the user can add one to the dashboard) has 16
`var(--cui-*)` references missing the cui-ui-style hex fallback.

The dialog is directly tile-related: it renders one cell per
registered tile (using `tile.displayName` and the static icon),
so its chrome must match the rest of the design system.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss`
- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss
```

Returns 16 matches.

## Expected

Each `var()` reference includes its design-system hex fallback:

- `var(--cui-surface, #1E1E1E)`
- `var(--cui-surface-2, #242424)`
- `var(--cui-text-primary, #FFFFFF)`
- `var(--cui-text-secondary, #B0B0B0)`
- `var(--cui-divider, #3A3A3A)`
- `var(--cui-accent, #FF4081)`
- `var(--cui-primary, #9FA8DA)`
- `var(--cui-primary-dim, #3F51B5)`

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing add-tile-dialog specs continue to pass.
