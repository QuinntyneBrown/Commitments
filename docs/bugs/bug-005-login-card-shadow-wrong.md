# Bug 005 — Login card shadow test description was stale

**Status**: Fixed

## Description

The e2e test `login.spec.ts` checked for `rgba(0, 0, 0, 0.6) 0px 6px 12px` but the
`--cui-shadow-login-card` token renders as `rgba(0, 0, 0, 0.7) 0px 8px 24px`. The
test description "uses the floating shadow token" was also misleading.

## Fix

The remote updated the test to assert the actual token value and renamed the test to
"card uses the dedicated --cui-shadow-login-card token from the design". No CSS change needed.
