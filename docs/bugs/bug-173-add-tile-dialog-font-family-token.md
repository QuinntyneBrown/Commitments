---
id: bug-173
title: add-tile-dialog SCSS uses literal `font-family: Inter` instead of --cui-font-display token
status: Fixed
---

# Bug 173 — add-tile-dialog font-family should reference `--cui-font-display`

**Status**: Fixed

## Fix

One-line SCSS swap on `.add-tile-dialog` to
`var(--cui-font-display, 'Inter')`. The cells inherit, so no
other rule needed updating. 373/373 workspace tests green.

## Description

`add-tile-dialog.component.scss` line 16 declares the dialog
`font-family` literally:

```scss
.add-tile-dialog {
  ...
  font-family: Inter, Roboto, "Helvetica Neue", Arial, sans-serif;
}
```

Same pattern that bug-172 fixed for dashboard-shell. Switch
to the design-system token form so the dialog tracks any
future font-family change in `_tokens.scss`:

```scss
font-family: var(--cui-font-display, 'Inter');
```

The descendant cells use `font-family: inherit` which already
picks up whatever the dialog root sets, so only one line
needs to change.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss`

## Reproduction

```bash
grep -n 'font-family: Inter' frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss
```

Returns one match.

## Expected

The `.add-tile-dialog` rule uses
`var(--cui-font-display, 'Inter')`. Regression-guard spec
asserts the literal is gone.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
