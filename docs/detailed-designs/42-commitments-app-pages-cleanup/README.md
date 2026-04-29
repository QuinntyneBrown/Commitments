# commitments-app Pages Cleanup — Detailed Design

**Status:** Complete

## 1. Overview

Goal: complete the app-level cleanup deferred from designs 34, 38, and 40. All feature page components already live in their libraries. This design:

1. Exports the missing page components from `@commitments/tracking-feature` and `@commitments/cards-feature` public APIs.
2. Updates `commitments-app/src/app/app.routes.ts` to import all remaining components (tracking, cards, identity) from their feature libraries.
3. Updates `commitments-app/src/app/app.routes.spec.ts` with matching imports from libraries.
4. Deletes every local page folder under `commitments-app/src/app/pages/`.

After this slice `commitments-app/src/app/pages/` is empty and `app.routes.ts` is a flat list of `...libraryRoutes` plus one `DashboardShellComponent` entry — matching the arc goal stated in design 41's index.

## 2. Changes

### 2.1 public-api additions

`@commitments/tracking-feature`:
```ts
export * from './lib/pages/activities-page/activities-page.component';
export * from './lib/pages/to-dos-page/to-dos-page.component';
```

`@commitments/cards-feature`:
```ts
export * from './lib/pages/cards-page/cards-page.component';
export * from './lib/pages/card-layouts-page/card-layouts-page.component';
```

### 2.2 app.routes.ts

Replace local imports for login/profiles/my-profile, activities/to-dos, cards/card-layouts with imports from feature libraries. Use component references directly; keep `login` at the top level and `profiles`/`my-profile` inside `DashboardLayoutComponent` children.

### 2.3 app.routes.spec.ts

Update imports to pull `LoginPageComponent`, `ProfilesPageComponent`, `MyProfilePageComponent` from `@commitments/identity-feature`; `ActivitiesPageComponent`, `ToDosPageComponent` from `@commitments/tracking-feature`; `CardsPageComponent`, `CardLayoutsPageComponent` from `@commitments/cards-feature`.

### 2.4 tsconfig.spec.json

Add path mappings for `@commitments/tracking-feature`, `@commitments/cards-feature` (identity-feature is already present).

### 2.5 Page folder deletion

Delete all folders under `commitments-app/src/app/pages/` — every page is now owned by a feature library.

## 3. Why "Radically Simple"

Zero new logic. Pure import re-pointing + file deletion. The tests stay green because the library components are the same components the spec was already asserting against — just imported from a different path.
