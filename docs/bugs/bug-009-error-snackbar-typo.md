# Bug 009 — Error snackbar action text has typo and missing space

**Status**: Fixed

## Description

`ErrorService.handle$` uses `'An error ocurr.Try it again.'` as the default action label.
The word "occurred" is truncated to "ocurr" and the period is jammed against "Try" with no space.
The same string is mirrored verbatim in the English i18n translation file, so the snackbar
button always reads "An error ocurr.Try it again." in the browser.

## Affected files

- `frontend/projects/commitments-app/src/app/core/error.service.ts`
- `frontend/projects/commitments-app/src/assets/i18n/en.json`
- `frontend/projects/commitments-app/src/assets/i18n/fr.json`

## Fix

Change the string to `'An error occurred. Try it again.'` in the service and both i18n files.
