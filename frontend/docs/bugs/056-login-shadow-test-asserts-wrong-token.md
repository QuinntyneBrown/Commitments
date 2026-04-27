# 056 — Login card shadow test asserts the wrong design token

## Status

FIXED — `login.spec.ts:56` passes on lg-desktop.

## Symptom

`login.spec.ts:56` — `card uses the floating shadow token from the design`:

```
Expected substring: "rgba(0, 0, 0, 0.6) 0px 6px 12px"
Received string:    "rgba(0, 0, 0, 0.7) 0px 8px 24px 0px"
```

## Root cause

The login card SCSS (`login-page.component.scss`) uses the **dedicated**
shadow token defined for it in `tokens/_tokens.scss`:

```scss
mat-card { … box-shadow: var(--cui-shadow-login-card); }
```

Where `--cui-shadow-login-card: 0 8px 24px #000000B3` (rgba 0.7). The test
asserts `--cui-shadow-floating` (`0 6px 12px rgba(0, 0, 0, 0.6)`) — a
different, generic floating-element token.

The "Received" value is exactly what the dedicated token resolves to. The
implementation is correct; the test names + asserts the wrong token.

## Fix

Update the test to assert the dedicated `--cui-shadow-login-card` value
(`rgba(0, 0, 0, 0.7) 0px 8px 24px`) and rename the test to make the token
intent explicit.

## Resolution

- [x] Failing test verified pre-fix.
- [x] Test updated to assert `rgba(0, 0, 0, 0.7) 0px 8px 24px` and
      renamed to mention `--cui-shadow-login-card`.
- [x] Test verified passing post-fix.
