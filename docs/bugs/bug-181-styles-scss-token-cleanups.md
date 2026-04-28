---
id: bug-181
title: styles.scss has lowercase #f44336 fallback and literal font-family Inter
status: Open
---

# Bug 181 — `styles.scss` token cleanups

## Description

Two cleanups in the global stylesheet:

1. **Lowercase fallback hex**: line 23 has
   `var(--cui-warn, #f44336)` — the canonical token value is
   `#F44336`. Every other consumer of `--cui-warn` uses the
   uppercase form. Normalize for consistency.

2. **Literal font-family**: line 11 has the literal
   `font-family: Inter, Roboto, 'Helvetica Neue', sans-serif;`.
   Continues the bug-172/173/174/175 arc — should use
   `var(--cui-font-display, 'Inter')`.

## Affected files

- `frontend/projects/commitments-app/src/styles.scss`

## Reproduction

```bash
grep -n 'f44336\|font-family: Inter' frontend/projects/commitments-app/src/styles.scss
```

Returns the two lines.

## Expected

```scss
body, html {
  ...
  font-family: var(--cui-font-display, 'Inter');
  ...
}

.snackbar--error .mdc-snackbar__surface {
  background-color: var(--cui-warn, #F44336) !important;
  ...
}
```

## Verification

- New regression spec asserts both fixes.
- All other tests continue to pass.
