# 059 — Login shadow test asserts the pre-token-change value

## Status

FIXED — obsolete. Sonnet's commit `dc36bdb fix(login): remove duplicate
box-sizing; revert shadow token to match remote test` reverted
`--cui-shadow-login-card` back to `0 8px 24px #000000B3`. The test now
passes against the original assertion. No change to the test was needed.

## Symptom

`login.spec.ts:56` — `card uses the dedicated --cui-shadow-login-card token`:

```
Expected substring: "rgba(0, 0, 0, 0.7) 0px 8px 24px"
Received string:    "rgba(0, 0, 0, 0.6) 0px 6px 12px 0px"
```

## Root cause

In `tokens/_tokens.scss`, both shadow tokens are now identical:

```scss
--cui-shadow-floating:    0 6px 12px rgba(0, 0, 0, 0.6);
--cui-shadow-login-card:  0 6px 12px rgba(0, 0, 0, 0.6);
```

`--cui-shadow-login-card` was updated in `5fb1374 fix(bugs 004-007): …`
to align with `--cui-shadow-floating`. Bug 056's earlier test fix
asserted the prior login-card value (`0 8px 24px #000000B3`).

The SCSS still reads `box-shadow: var(--cui-shadow-login-card)`, so the
dedicated token name still serves as documentation. The assertion just
needs to match the current value.

## Fix

Update the assertion to `rgba(0, 0, 0, 0.6) 0px 6px 12px` (the current
resolved value of `--cui-shadow-login-card`).

## Resolution

- [x] Failing test verified pre-fix.
- [x] Token reverted upstream (`dc36bdb`).
- [x] Test verified passing post-revert.
