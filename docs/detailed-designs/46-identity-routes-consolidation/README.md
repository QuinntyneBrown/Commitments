# Design 46 — Identity Routes Consolidation

Status: Complete

## Context

Every other feature library uses a `...libraryRoutes` spread in `app.routes.ts`:
`...behavioursRoutes`, `...cardsRoutes`, `...settingsRoutes`, etc.

The identity feature is the exception: `profiles` and `my-profile` are registered
manually in the app with direct component imports. `identityRoutes` exists in the
library but currently includes `login`, which belongs outside the layout shell.

## Goal

Make identity routes consistent with all other feature libraries — one `...identityRoutes`
spread inside the layout shell children, no manual component registration needed.

## Design

### `commitments-identity-feature/src/lib/routes.ts` (update)

Remove `login` from `identityRoutes` — login is a public route, not a layout-shell child:

```ts
export const identityRoutes: Routes = [
  { path: 'profiles',   component: ProfilesPageComponent },
  { path: 'my-profile', component: MyProfilePageComponent },
];
```

### `app.routes.ts` (update)

Replace manual `profiles`/`my-profile` entries with `...identityRoutes`:

```ts
import { authGuard, identityRoutes, LoginPageComponent } from '@commitments/identity-feature';

export const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  {
    path: '',
    component: DashboardLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: DashboardShellComponent },
      ...identityRoutes,
      ...settingsRoutes,
      ...behavioursRoutes,
      ...frequenciesRoutes,
      ...commitmentsRoutes,
      ...trackingRoutes,
      ...notesRoutes,
      ...cardsRoutes,
    ]
  }
];
```

### `identity-feature-host/src/app/app.routes.ts` (update)

Update host to also drop login from its routes (login is an app-level concern):

```ts
export const routes: Routes = [
  { path: '', redirectTo: 'profiles', pathMatch: 'full' },
  ...identityRoutes,
];
```

## Acceptance tests

### `app.routes.spec.ts` additions

- Dashboard layout children contain `...identityRoutes` entries
- `/profiles` still maps to `ProfilesPageComponent`
- `/my-profile` still maps to `MyProfilePageComponent`
- No direct `ProfilesPageComponent` / `MyProfilePageComponent` registration in children
  (they come via spread)

## Implementation steps

1. Write failing spec additions
2. Commit + push
3. Update `identityRoutes`, `app.routes.ts`, host routes
4. Verify tests pass → commit + push
