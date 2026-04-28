---
id: bug-163
title: Remove 6 dead form-related UI components from @commitments/ui
status: Open
---

# Bug 163 — Drop dead form/feedback UI components (button, chip, text-input, radio-group, snackbar, dialog-shell)

## Description

Continuing the bug-162 cleanup arc. Six more components are
exported from `@commitments/ui` but no consumer in the
workspace imports them:

| Component | Selector |
|-----------|----------|
| `ButtonComponent` | `cui-button` |
| `ChipComponent` | `cui-chip` |
| `TextInputComponent` | `cui-text-input` |
| `RadioGroupComponent` | `cui-radio-group` |
| `SnackbarComponent` | `cui-snackbar` |
| `DialogShellComponent` | `cui-dialog-shell` |

Cross-repo grep confirms each selector and class name is
referenced only inside the component's own `.ts`/`.html`/`.scss`
files. No `.spec.ts` files exist for any of them.

## Affected files (18 + 6 export lines)

For each component, three files in
`frontend/projects/commitments-ui/src/lib/<dir>/`:
- `<dir>.component.ts`
- `<dir>.component.html`
- `<dir>.component.scss`

Plus 6 `export *` lines removed from `public-api.ts`.

## Reproduction

```bash
grep -rn 'cui-button\|cui-chip\|cui-text-input\|cui-radio-group\|cui-snackbar\|cui-dialog-shell' frontend/projects/commitments-app
```

Returns no matches.

## Expected

- 18 component files removed.
- 6 lines removed from `public-api.ts`.
- Regression-guard spec asserts each directory's
  `.component.ts` no longer exists.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
