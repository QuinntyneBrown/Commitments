# Design 45 — Auth Guard

Status: Complete

## Context

After design 44 removed the old app-level `auth.guard.ts` along with its now-dead
infrastructure, no route guard exists to redirect unauthenticated users to `/login`.
Any visitor can navigate to `/commitments`, `/activities`, etc. without a token.

## Goal

Add a minimal `authGuard` to the `commitments-identity-feature` library and apply it
to the dashboard layout route so that unauthenticated users are sent to `/login`.

## Design

### `commitments-identity-feature/src/lib/guards/auth.guard.ts`

```ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = () =>
  !!localStorage.getItem('accessTokenKey') || inject(Router).createUrlTree(['/login']);
```

One exported function, no class, no service injection, no side effects.

### `commitments-identity-feature/src/public-api.ts`

Export `authGuard`.

### `app.routes.ts` — apply guard

```ts
{
  path: '',
  component: DashboardLayoutComponent,
  canActivate: [authGuard],
  children: [...]
}
```

## Acceptance tests

### Unit — `auth.guard.spec.ts` (in identity feature)

| Test | Expected |
|------|----------|
| token present | returns `true` |
| token absent | returns `UrlTree` pointing at `/login` |

### Unit — `app.routes.spec.ts` (in commitments-app)

| Test | Expected |
|------|----------|
| dashboard layout route has `canActivate: [authGuard]` | assertion passes |

## Implementation steps

1. Write failing specs
2. Commit + push
3. Create `auth.guard.ts`, export from `public-api.ts`, update `app.routes.ts`
4. Verify tests pass → commit + push
