---
id: bug-175
title: login-page SCSS uses literal `font-family: Inter` instead of --cui-font-display token
status: Open
---

# Bug 175 — login-page font-family should reference `--cui-font-display`

## Description

`login-page.component.scss` declares `font-family: Inter, sans-serif`
in three selectors (lines 27, 36, 58). Continues the
bug-172/173/174 pattern: switch each to
`var(--cui-font-display, 'Inter')`.

```scss
.mat-typography h2 {
  ...
  font-family: var(--cui-font-display, 'Inter');
  ...
}
mat-card-title { ...font-family: var(--cui-font-display, 'Inter'); ... }
button { ...font-family: var(--cui-font-display, 'Inter'); ... }
```

## Affected files

- `frontend/projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`

## Reproduction

```bash
grep -n 'font-family: Inter' frontend/projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss
```

Returns 3 matches.

## Expected

All three `font-family` declarations use
`var(--cui-font-display, 'Inter')`. Regression-guard spec in
`app.routes.spec.ts` asserts the literals are gone.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
