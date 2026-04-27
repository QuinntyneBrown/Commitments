---
id: 002
title: LoginPageComponent imports a non-existent module
status: open
discovered: 2026-04-26
flow: authentication
severity: critical
---

# LoginPageComponent imports a non-existent module

## Summary

`LoginPageComponent` imports `AuthService` from `'../../../core/auth'`, but
the actual file is `'../../../core/auth.service'`. The component therefore
fails to type-check (TS2307 "Cannot find module"), and the cascade leaves
`AuthService` typed as `unknown` so every property access (`logout`,
`tryToLogin`, `profileId`) is an error too.

This bug blocks both unit tests that import the component and the e2e
test that needs to render `/login`.

## Reproduction

1. `npm test -- --testPathPattern=app.routes`
2. **Expected:** test runs (and may fail on assertions, but compiles).
3. **Actual:** `FAIL ... Cannot find module '../../../core/auth' or its
   corresponding type declarations.`

## Evidence

`frontend/projects/commitments-app/src/app/pages/login/login-page/login-page.component.ts:15`:

```ts
import { AuthService } from '../../../core/auth';
```

`frontend/projects/commitments-app/src/app/core/` contains
`auth.service.ts` and `auth.guard.ts`, but no `auth.ts`.

## Fix outline (radically simple)

Change the import from `'../../../core/auth'` to
`'../../../core/auth.service'`.

## Tests to add (failing first)

The same `app.routes.spec.ts` that targets bug #001 already fails on
compile because of this bug — fixing this is a prerequisite for that test
to even run.
