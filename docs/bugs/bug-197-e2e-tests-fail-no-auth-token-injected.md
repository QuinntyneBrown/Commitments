---
id: bug-197
title: All dashboard/shell e2e tests fail because DashboardPage.goto() never injects an access token — auth guard always redirects to /login
status: Open
---

## Symptom

Every e2e test that uses `DashboardPage` (or any non-login POM) fails immediately:

```
Error: [locator].toBeVisible() failed
Locator: getByTestId('dashboard-shell')
Expected: visible
Timeout: 5000ms
```

The app navigates to `/` without an access token in `localStorage`, so the `authGuard` (which checks `!!localStorage.getItem('accessTokenKey')`) redirects to `/login`. The dashboard never renders.

## Root cause

`DashboardPage.goto()` clears `commitments.layout.live` via `addInitScript` but never seeds the `accessTokenKey`. All non-login tests assume auth state already exists, but nothing sets it.

## Fix

Add a Playwright global setup (`auth.setup.ts`) that:
1. Calls `POST https://localhost:63713/api/v1.0/users/token` with the seeded credentials (`quinntynebrown@gmail.com` / `P@ssw0rd`)
2. Saves the resulting `accessToken` in a Playwright `storageState.json` (under `localStorage` for `http://127.0.0.1:4200`)

Configure `playwright.config.ts` to:
- Run `auth.setup.ts` as a `setup` project
- Set `storageState: 'playwright/.auth/user.json'` in the `use` block of each non-setup project
