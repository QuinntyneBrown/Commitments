# Profile switching

> **Status (2026-04-27):** the `/profiles` and `/my-profile` routes both resolve to `PlaceholderPageComponent` ("Coming soon"). The header in the active shell (`DashboardLayoutComponent`) shows a hard-coded `Quinn Brown` profile name and an empty avatar slot — it does **not** yet read from `currentProfile$` or react to a profile click. The catalog page, create-profile dialog, header avatar/name binding, and SignalR profile-update path described below are the **intended** implementation; the backend Identity module, the create-profile dialog, `profile.service.ts`, `app-store.ts`, and `headers.interceptor.ts` still ship in source. Treat the steps below as the contract a Playwright test will exercise once the placeholders are replaced and the active shell is wired to the profile store.

## Summary

A signed-in user owns one or more profiles. Domain data (commitments, activities, notes, todos, tags, cards, dashboards) is scoped to the active profile. The user can view their current profile, switch to another profile, and create a new one. The header avatar and name reflect the active profile, and the API includes the active `profileId` on every request via the headers interceptor.

## Surface area

- Pages: `pages/my-profile/...`, `pages/profiles/...` (currently shadowed by `PlaceholderPageComponent`).
- Header (active shell): `components/dashboard-layout/dashboard-layout.component.{ts,html}` — today the profile name is hard-coded (`Quinn Brown`) and the avatar slot has no `--background-image-url` binding; once wired, avatar + name update on profile change and clicking the name navigates to `/my-profile`.
- Header (legacy, not routed): `components/master-page/master-page.component.{ts,html}` — kept here for context; no longer reachable from `app.routes.ts`.
- Dialog: `components/create-profile-dialog/...`
- Services: `services/profile.service.ts`, `app-store.ts` (BehaviorSubject `currentProfile$`).
- Storage: `localStorage[currentProfileIdKey]`.
- Backend: `Identity.Module` — `GET/POST /api/profiles`, `GET /api/profiles/current`.
- Cross-cutting: `core/headers.interceptor.ts` injects `ProfileId` header on every request; `HttpContextAccessorExtensions.GetProfileId()` reads it.

## Preconditions

- User is authenticated (see [`authentication`](../authentication/README.md)).
- At least one profile exists for the user.

## Steps

1. **Header reflects the current profile.**
   - From the dashboard, observe the toolbar.
   - **Assert:** the toolbar shows `Commitments` (app title) and the active profile name to the right; the round avatar slot has `--background-image-url` CSS variable set to the profile's `avatarUrl`.

2. **Open My Profile by clicking the name.**
   - Click the profile name in the toolbar.
   - **Assert:** the app navigates to `/my-profile` and renders the profile detail.

3. **Open the Profiles list.**
   - Open the sidenav and click `Profiles`.
   - **Assert:** the app navigates to `/profiles` and renders the profile catalog.

4. **Create a new profile.**
   - Click the floating `+` action and complete the create-profile dialog with a new name and avatar.
   - Click the dialog's primary action.
   - **Assert:** dialog closes; the new profile appears in the catalog; the API call to `POST /api/profiles` returned 201 with a new `profileId`.

5. **Switch to the new profile.**
   - From the catalog, select the new profile (catalog row click or "use" affordance).
   - **Assert:** `localStorage[currentProfileIdKey]` updates to the new id; toolbar name + avatar update; subsequent calls (e.g. visiting `/commitments`) include `ProfileId: <new id>` in the `Request.Headers` and return only that profile's data.

6. **SignalR pushes a profile update.**
   - With the dashboard open, trigger an out-of-band profile update for the active profile (e.g. via API).
   - **Assert:** `HubClient.messages$` fires; the toolbar avatar/name re-renders without a page reload (handled in `master-page.component.ts`).

## Selectors

| Need | Selector |
| --- | --- |
| Toolbar profile name | `getByTestId('dashboard-layout-profile-name')` (currently a static span; will become a clickable affordance once wired to `/my-profile`) |
| Toolbar avatar | `getByTestId('dashboard-layout-avatar')` |
| Profiles sidenav link | `getByRole('button', { name: /profiles/i })` |
| Create-profile FAB | first `mat-fab` on the `/profiles` page |
| Create dialog primary action | role `dialog`, primary `button` (text `Save` / `Create`) |

> **No `data-testid` on toolbar or profile rows yet.** Add `data-testid="toolbar-profile-name"` and `data-testid="profile-row-<id>"` when authoring a test.

## Edge cases

- User has exactly one profile → switch UI is hidden / no-op.
- Avatar URL 404 → CSS background fails silently; image fallback should still render.
- `currentProfileIdKey` cleared while user remains on a page → next API call lacks `ProfileId` and is rejected; flow should redirect or refetch.
