---
id: bug-184
title: dashboard-grid edit-mode border-radius uses literal 8px instead of --cui-radius-md token
status: Fixed
---

# Bug 184 — dashboard-grid edit-mode `border-radius` should use `--cui-radius-md`

**Status**: Fixed

## Fix

```scss
- border-radius: 8px;
+ border-radius: var(--cui-radius-md, 8px);
```

384/384 workspace tests green.

## Description

`dashboard-grid.component.scss` line 28:

```scss
.dashboard-grid--edit-mode gridster-item {
  border: 2px solid var(--cui-accent, #FF4081);
  border-radius: 8px;
}
```

The `8px` is the canonical value of `--cui-radius-md` from
`_tokens.scss`. Routing through the token keeps edit-mode
border-radius in sync with any future radius update from the
design system.

```scss
border-radius: var(--cui-radius-md, 8px);
```

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`

## Reproduction

```bash
grep -n 'border-radius: 8px' frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss
```

Returns one match.

## Expected

```scss
border-radius: var(--cui-radius-md, 8px);
```

A regression-guard spec asserts the literal `border-radius: 8px;`
form is gone (replaced by the token form).

## Verification

- New regression spec confirms the fix.
- All other tests continue to pass.
