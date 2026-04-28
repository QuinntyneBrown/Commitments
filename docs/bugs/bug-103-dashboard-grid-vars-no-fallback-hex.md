---
id: bug-103
title: dashboard-grid.component.scss var() calls omit the design-system fallback hex
status: Fixed
---

# Bug 103 — dashboard-grid var() fallback hex

**Status**: Fixed

## Fix

Added the design-system hex fallback to six `var()` references
in `dashboard-grid.component.scss`:

- `var(--cui-bg, #121212)` (gridster background)
- `var(--cui-accent, #FF4081)` (edit border, drag chip)
- `var(--cui-warn, #F44336)` (remove chip)
- `var(--cui-text-secondary, #B0B0B0)` (drag icon, empty copy)
- `var(--cui-text-primary, #FFFFFF)` (drag label, empty title)

Created a minimal `dashboard-grid.component.spec.ts` with the
bug-103 regex guard (no spec previously existed).
300/300 workspace tests green. Brings the dashboard-framework
host into the same fallback convention as the dashboard-plugin
tiles (bug-096–102).

## Description

Continuing the bug-096–102 series outside the dashboard-plugin
tiles: `dashboard-framework`'s
`dashboard-grid.component.scss` (which owns the gridster shell
and the edit-mode `tile-chrome` chips) has six `var(--cui-*)`
references missing the cui-ui-style hex fallback:

```scss
gridster                    { background: var(--cui-bg); }
gridster-item.in-edit       { border: 2px solid var(--cui-accent); }
.tile-chrome__drag          { background: var(--cui-accent); }
.tile-chrome__remove        { background: var(--cui-warn); }
.tile-chrome__drag-icon     { color: var(--cui-text-secondary); }
.tile-chrome__drag-label    { color: var(--cui-text-primary); }
```

Tokenizing with fallbacks documents the intended palette and
keeps the chrome visible if the global tokens stylesheet is
late or scoped out.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss
```

Returns six matches.

## Expected

- `var(--cui-bg, #121212)` (gridster background)
- `var(--cui-accent, #FF4081)` (edit-mode border, drag chip)
- `var(--cui-warn, #F44336)` (remove chip)
- `var(--cui-text-secondary, #B0B0B0)` (drag icon)
- `var(--cui-text-primary, #FFFFFF)` (drag label)

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing dashboard-grid specs continue to pass.
