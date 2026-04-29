---
id: bug-195
title: All backend controller routes missing 'v' prefix and controllers using [controller] produce singular names — frontend services call api/v1.0/{plural} but backend exposes api/1.0/{singular}
status: Fixed
---

## Symptom

Every API call from the frontend fails with 404. The Angular services consistently use `api/v1.0/{pluralResource}` (e.g. `api/v1.0/behaviours`, `api/v1.0/users/token`) but the backend controllers are routed as `api/1.0/{singularResource}` (e.g. `api/1.0/behaviour`, `api/1.0/user`).

Two issues:

1. **`v` prefix**: routes use `{version:apiVersion}` which matches `1.0` but NOT `v1.0`.
2. **Singular vs plural**: controllers using `[controller]` produce singular names (`user`, `behaviour`, etc.) while services use plural (`users`, `behaviours`, etc.).

## Root cause

All controllers use `[Route("api/{version:apiVersion}/[controller]")]`. `Asp.Versioning 8.x` does not match `v1.0` with this constraint — only `1.0`. The `[controller]` token strips the "Controller" suffix and lowercases, producing `user` not `users`.

Some controllers already use explicit plural routes (`notes`, `tags`, `todos`, `weekly-focus`, `goal-progress`, `monthly-progress`, `relations`) — they just need the `v` prefix.

## Fix

1. Change all route templates: `api/{version:apiVersion}/` → `api/v{version:apiVersion}/`
2. For controllers using `[controller]`, replace with explicit plural routes matching frontend:
   - `UserController` → `users`
   - `BehaviourController` → `behaviours`
   - `BehaviourTypeController` → `behaviourTypes`
   - `CommitmentController` → `commitments`
   - `FrequencyController` → `frequencies`
   - `FrequencyTypeController` → `frequencyTypes`
   - `ActivityController` → `activities`
   - `ProfileController` → `profiles`
   - `CardController` → `cards`
   - `CardLayoutController` → `cardLayouts`
   - `DashboardController` → `dashboards`
   - `DashboardCardController` → `dashboardCards`
   - `AchievementController` → `achievements`
