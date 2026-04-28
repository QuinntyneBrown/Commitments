---
id: bug-172
title: dashboard-shell SCSS uses literal `font-family: Inter` instead of --cui-font-display token
status: Open
---

# Bug 172 — dashboard-shell font-family should reference `--cui-font-display`

## Description

`dashboard-shell.component.scss` declares `font-family` with a
literal `Inter, ...` font stack four times:

```scss
:host {
  ...
  font-family: Inter, Roboto, "Helvetica Neue", Arial, sans-serif;
}

.dashboard-shell__edit-pill {
  ...
  font-family: Inter, sans-serif;
}

.dashboard-shell__edit-label {
  ...
  font-family: Inter, sans-serif;
}

.dashboard-shell__done {
  ...
  font-family: Inter, sans-serif;
}
```

Other commitments-ui components (`tile-shell`, `metric-header`,
`mode-toggle`, etc.) use the token form:

```scss
font-family: var(--cui-font-display, 'Inter');
```

Switch the dashboard-shell to the token form so any future
font-family change in `_tokens.scss` propagates here too. The
literal stays as the fallback for browsers without the
custom property defined.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.scss`

## Reproduction

```bash
grep -n 'font-family: Inter' frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.scss
```

Returns 4 matches.

## Expected

Each `font-family` declaration uses
`var(--cui-font-display, '<inter-stack>')`. The 4 literal
`font-family: Inter, ...` lines are replaced.

## Verification

- New regression spec asserts the SCSS file contains zero
  literal `font-family: Inter` declarations (anchored to start
  of property).
- All other tests continue to pass.
