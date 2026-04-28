---
id: bug-176
title: add-tile-dialog uses #fff as --cui-on-primary fallback but token is #1A1B23
status: Open
---

# Bug 176 — add-tile-dialog `--cui-on-primary` fallback mismatch

## Description

`add-tile-dialog.component.scss` line 182:

```scss
.add-tile-dialog__btn--primary {
  background: var(--cui-primary, #9FA8DA);
  color: var(--cui-on-primary, #fff);
  border: none;
}
```

The token in `_tokens.scss` is:

```scss
--cui-on-primary: #1A1B23;  // dark navy
```

…not white. If the token is ever undefined (e.g., the
`_tokens.scss` import is missing for some host context), the
fallback hardcodes white text on light-purple background —
poor contrast and a clear accessibility regression vs. the
intended dark-on-light pairing.

The correct fallback should match the token: `#1A1B23`.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss`

## Reproduction

```bash
grep -n 'var(--cui-on-primary,' frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss
```

Returns one match with `#fff` as the fallback.

## Expected

```scss
color: var(--cui-on-primary, #1A1B23);
```

A regression-guard spec asserts the fallback now matches the
canonical token value.

## Verification

- New regression spec confirms the corrected fallback.
- All other tests continue to pass.
