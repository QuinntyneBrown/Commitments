---
id: 001
title: /login route does not render the LoginPageComponent
status: open
discovered: 2026-04-26
flow: authentication
severity: critical
---

# /login route does not render the LoginPageComponent

## Summary

Navigating to `/login` (or any other path) shows the dashboard shell instead of the
`LoginPageComponent`. The Angular router is not provided, no routes are
registered, and `AppComponent`'s template renders the dashboard shell directly
with no `<router-outlet>`. As a result, the documented authentication flow (see
`docs/flows/authentication/README.md`) cannot be exercised at all.

## Reproduction

1. Start the frontend: `npm run start` (port 4200, IPv4).
2. Navigate to `http://127.0.0.1:4200/login`.
3. **Expected:** the login card from `login-page.component.html` renders
   (matches `docs/ui-design.pen` → frame `Login — LG (1280)` (id `8xz6c`)).
4. **Actual:** the dashboard tiles render — the same content as `/`.

## Evidence

- `frontend/projects/commitments-app/src/app/app.routes.ts` exports
  `Routes = []` (empty).
- `frontend/projects/commitments-app/src/app/app.component.html` is just
  `<commitments-dashboard-shell></commitments-dashboard-shell>` — no
  `<router-outlet>`.
- `frontend/projects/commitments-app/src/app/app.config.ts` contains no
  `provideRouter(routes)` call.
- Screenshot: `screenshots/login-actual-1280.png` (captured at `/login`,
  shows the dashboard).
- Design reference: `docs/ui-design.pen` → node `8xz6c`
  ("Login — LG (1280)").

## Root cause

Routing was never wired up after the modular monolith refactor (commit
`6256ed5`). The app renders the dashboard shell unconditionally.

## Fix outline (radically simple)

1. `app.routes.ts` — register two routes:
   - `path: 'login'` → `LoginPageComponent`
   - `path: ''` → `DashboardShellComponent`
2. `app.component.html` — replace dashboard shell tag with
   `<router-outlet></router-outlet>`.
3. `app.component.ts` — drop the `DashboardShellComponent` import; import
   `RouterOutlet` instead.
4. `app.config.ts` — add `provideRouter(routes)`.

## Tests to add (failing first)

- Unit: `app.routes.spec.ts` — assert that `routes` contains a `login` entry
  pointing at `LoginPageComponent`, and that `''` points at
  `DashboardShellComponent`.
- E2E: `e2e/login.spec.ts` (Page Object Model in `e2e/pages/login.page.ts`) —
  navigate to `/login`, expect username/password inputs and the Submit button
  to be visible.
