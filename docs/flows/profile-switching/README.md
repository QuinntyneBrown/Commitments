# Profile switching

## Summary

A signed-in user owns one or more profiles. Domain data (commitments, activities, notes, todos, tags, cards, dashboards) is scoped to the active profile. The user can view their current profile, switch to another profile, and create a new one. The header avatar and name reflect the active profile, and the API includes the active `profileId` on every request via the headers interceptor.

## Surface area

- Pages: `pages/my-profile/...`, `pages/profiles/...`
- Header: `components/master-page/master-page.component.{ts,html}` — avatar + name update on profile change, click navigates to `/my-profile`.
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
| Toolbar profile name link | `.profile-name` (in `master-page.component.html`) |
| Profiles sidenav link | `getByRole('button', { name: /profiles/i })` |
| Create-profile FAB | first `mat-fab` on the `/profiles` page |
| Create dialog primary action | role `dialog`, primary `button` (text `Save` / `Create`) |

> **No `data-testid` on toolbar or profile rows yet.** Add `data-testid="toolbar-profile-name"` and `data-testid="profile-row-<id>"` when authoring a test.

## Edge cases

- User has exactly one profile → switch UI is hidden / no-op.
- Avatar URL 404 → CSS background fails silently; image fallback should still render.
- `currentProfileIdKey` cleared while user remains on a page → next API call lacks `ProfileId` and is rejected; flow should redirect or refetch.
