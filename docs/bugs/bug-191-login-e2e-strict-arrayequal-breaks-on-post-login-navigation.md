---
id: bug-191
title: login-page e2e strict toEqual assertion fails when post-login navigation triggers additional API calls
status: Fixed
---

# Bug 191 — Login e2e spec uses strict `toEqual` and breaks when post-login navigation fires a profiles fetch

**Status**: Fixed

## Fix

`login-page.spec.ts` — change the assertion from strict array equality to `arrayContaining`:

```typescript
- expect(calls).toEqual([
-   { method: 'post', path: 'api/v1.0/users/token', body: { username: 'alice', password: 'pw' } }
- ]);
+ expect(calls).toEqual(expect.arrayContaining([
+   { method: 'post', path: 'api/v1.0/users/token', body: { username: 'alice', password: 'pw' } }
+ ]));
```

## Description

`login-page.spec.ts` recorded backend calls after `signIn()` and
asserted they matched exactly one entry — the token POST. The
`signIn()` POM helper awaits only the button click, not the
subsequent navigation. By the time `backendCalls()` is read, the
router has navigated to `/` → redirected to `/profiles` →
`ProfilesPageComponent.ngOnInit()` has fired `GET api/v1.0/profiles`.

The profiles fetch is correct behaviour; the test assertion is the
bug. The test name is "submits credentials to api/v1.0/users/token",
not "makes exactly one API call". Using `toEqual` with a literal
single-element array is an accidental exactness check that breaks
whenever any intentional post-login navigation triggers a fetch.

## Affected files

- `frontend/projects/commitments-identity-feature-host/e2e/login-page.spec.ts`

## Reproduction

```bash
cd frontend && npm run e2e:identity-host
```

Fails with `Received` containing an extra `{ method: 'get', path: 'api/v1.0/profiles' }` entry.

## Expected

The test asserts that `api/v1.0/users/token` was called with the
correct credentials, regardless of what other calls the post-login
navigation triggers.

## Verification

- `npm run e2e:identity-host` — 4/4 pass.
