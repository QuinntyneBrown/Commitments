---
id: bug-099
title: relations-tile.component.scss var() calls omit the design-system fallback hex
status: Fixed
---

# Bug 099 — relations var() fallback hex

**Status**: Fixed

## Fix

Added the design-system hex fallback to three `var()` references
in `relations-tile.component.scss`:

- `var(--cui-text-secondary, #B0B0B0)` (label, .empty)
- `var(--cui-text-primary, #FFFFFF)` (value via <strong>)

Same shape as bug-096/097/098. The bug-099 spec adds a
forward-only regex guard. 296/296 workspace tests green.

## Description

Same shape as bug-096/097/098: three `var(--cui-*)` references
in `relations-tile.component.scss` are missing the cui-ui-style
hex fallback:

```scss
.relations          { color: var(--cui-text-secondary); }
.relations strong   { color: var(--cui-text-primary); }
.empty              { color: var(--cui-text-secondary); }
```

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.scss
```

Returns three matches.

## Expected

- `var(--cui-text-secondary, #B0B0B0)` (label + empty copy)
- `var(--cui-text-primary, #FFFFFF)` (value)

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- Existing relations-tile specs continue to pass.
