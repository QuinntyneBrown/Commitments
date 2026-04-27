---
id: bug-015
title: Error snackbar renders white background and uses verbose text as action button label
status: In Progress
---

# Bug 015 — Error snackbar white background + wrong action label

**Status**: In Progress

## Description

When a login error occurs the `MatSnackBar` renders with:
1. **White background** — should be dark (`#333333` base / `$warn` red for errors per design).
2. **"An error occurred. Try it again."** as the action button text — far too long for a
   button; the design (`USNXD`) specifies "RETRY" (or equivalent short label).

Design (`ui-design.pen` → `Mat-Snackbar/Error` → `USNXD`):
- `fill: "$warn"` (red), `$text-primary` (white) text
- Short action label ("RETRY"), matching `$text-primary`

## Affected files

- `frontend/projects/commitments-app/src/app/core/error.service.ts` — change default `action` to `'DISMISS'`; add `panelClass: ['snackbar--error']`
- `frontend/projects/commitments-app/src/assets/i18n/en.json` — replace old action key
- `frontend/projects/commitments-app/src/assets/i18n/fr.json` — replace old action key
- `frontend/projects/commitments-app/src/styles.scss` (or global CSS) — add `.snackbar--error` styles

## Fix

1. Change `action` default from the long description to `'DISMISS'`.
2. Add `panelClass: ['snackbar--error']` to the `open()` config.
3. Add `.snackbar--error` global style with `$warn` background and white text.
4. Update i18n files for the new key.
