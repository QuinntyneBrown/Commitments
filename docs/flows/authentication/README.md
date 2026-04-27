# Authentication

## Summary

A user signs in with username and password, gets routed to the dashboard, and can later sign out from the sidenav. Until a session is established, the app is unusable; after sign-out, all routes redirect back to `/login`.

## Surface area

- Page: `frontend/projects/commitments-app/src/app/pages/login/login-page/login-page.component.{ts,html}`
- Service: `core/auth.service.ts`, `core/jwt.interceptor.ts`, `core/headers.interceptor.ts`
- Backend: `Identity.Module` — `POST /api/users/token` (path is configured in `appsettings.json` → `Authentication:TokenPath`).
- Storage: `localStorage` keys `accessTokenKey`, `currentProfileIdKey` (see `core/constants.ts`).

## Preconditions

- API is reachable at the URL injected via the `baseUrl` provider (default `http://localhost:52748/`).
- A seeded user exists. In dev mode the platform should seed `quinntynebrown@gmail.com` / `password` (see open work item).
- `localStorage` is empty for `accessTokenKey` (test should clear it in `beforeEach`).

## Steps

1. **Land on the login screen.**
   - Navigate to `/login`.
   - **Assert:** the login card is centred on a dark `--cui-bg` (#121212) background, 480px wide on LG, with the design's surface fill (#1E1E1E), 8px corner radius, and floating box-shadow. It contains a brand title `Commitments` (Inter / 34 / 700), a `Login` mat-card-title (Inter / 20 / 500, secondary text colour), a `Username` input, a `Password` input (type `password`), and a full-width `Submit` button. The Submit button is disabled while the form is invalid. (See `e2e/login.spec.ts` for the asserted token values.)

2. **Submit empty form is blocked.**
   - Click `Submit` without filling either field.
   - **Assert:** no navigation occurs; the button remains disabled.

3. **Submit valid credentials.**
   - Fill `Username` with the seeded email and `Password` with `password`.
   - Click `Submit`.
   - **Assert:** the network call to `POST /api/users/token` returns 200 with an `access_token`; the token is stored in `localStorage[accessTokenKey]`; the app navigates to `/` (dashboard).

4. **Submit invalid credentials.**
   - Fill `Username` with the seeded email and `Password` with `wrong`.
   - Click `Submit`.
   - **Assert:** the network call returns 4xx; an error snackbar/toast is visible and the app remains on `/login`; `localStorage[accessTokenKey]` is unchanged.

5. **Sign out from the sidenav.**
   - From the dashboard, the sidenav is open by default; if collapsed, click the toolbar hamburger (`dashboard-layout-hamburger`) to expand it.
   - Click the `Logout` item (`sidenav-item-Logout`).
   - **Assert:** the app navigates to `/login`; `localStorage[accessTokenKey]` is cleared; reloading any protected route (e.g. `/commitments`) returns to `/login`.

## Selectors

| Need | Selector |
| --- | --- |
| Username input | `getByLabel('Username')` or `#username` |
| Password input | `getByLabel('Password')` or `#password` |
| Submit button | `getByRole('button', { name: /submit/i })` |
| Sidenav toggle | `getByTestId('dashboard-layout-hamburger')` |
| Logout link | `getByTestId('sidenav-item-Logout')` (anchor `routerLink="/login"` in `dashboard-layout.component.html`) |

> **No `data-testid` exists on the login form yet.** A Playwright author should add `data-testid="username"`, `data-testid="password"`, `data-testid="login-submit"` while authoring the test, and update this doc.

## Edge cases

- Network 500 from the token endpoint → user-visible error, `/login` retained.
- `accessTokenKey` already present but expired → request returns 401, app should clear the token and route to `/login`.
- Submit while offline → button stays enabled; error surface should be reachable.
