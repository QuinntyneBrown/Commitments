---
id: bug-174
title: dashboard-layout SCSS uses literal `font-family: Inter` instead of --cui-font-display token
status: Fixed
---

# Bug 174 — dashboard-layout font-family should reference `--cui-font-display`

**Status**: Fixed

## Fix

All four `font-family: Inter, Roboto, ...` literals (host,
brand, profile-name, sidenav-item) replaced with
`var(--cui-font-display, 'Inter')`. 374/374 workspace tests
green.

## Description

`commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.scss`
declares `font-family` literally in four selectors:

- `:host` (line 6)
- `.dashboard-layout__brand` (line 51)
- `.dashboard-layout__profile-name` (line 62)
- `.sidenav-item` (line 117)

Continues the bug-172 / bug-173 pattern: switch to the
design-system token.

```scss
font-family: var(--cui-font-display, 'Inter');
```

## Affected files

- `frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.scss`

## Reproduction

```bash
grep -n 'font-family: Inter' frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.scss
```

Returns 4 matches.

## Expected

All four `font-family` declarations use
`var(--cui-font-display, 'Inter')`. Regression-guard spec
asserts no bare literal remains.

## Verification

- New regression spec confirms removal of the literals.
- All other tests continue to pass.
