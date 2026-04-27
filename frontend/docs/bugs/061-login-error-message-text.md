# 061 — Failed login snackbar shows "Login Failed" instead of the friendlier translated message

## Status

OPEN — `login.spec.ts:60` (`failed login shows an error snackbar with correct spelling`).

## Symptom

```
Expected substring: "occurred"
Received string:    "Login Failed
                     Dismiss"
```

The test asserts the failure message contains "occurred" and never
contains the historical typo "ocurr." — both intended to enforce the
multi-language string already shipped in `assets/i18n/{en,fr}.json`:

```json
"An error occurred. Try it again.": "An error occurred. Try it again.",
```

## Root cause

`login-page.component.ts:94` calls
`this._errorService.handle$(errorResponse, 'Login Failed')`. The string
`'Login Failed'` is itself a translation key, so on failed login the
snackbar reads "Login Failed" rather than the friendlier sentence.

## Fix

Pass the existing translation key
`'An error occurred. Try it again.'`. It is already declared in both
`en.json` and `fr.json`, so en + fr users both get a localized,
typo-free sentence containing "occurred".

## Resolution

- [ ] Failing test verified pre-fix.
- [ ] Message key swapped.
- [ ] Test verified passing post-fix.
