# Tag management

> **Status (2026-04-27):** the `/tags` page is not registered in `app.routes.ts`, and the related `/notes/by-tag/:slug` route is also unregistered (`/notes` itself resolves to `PlaceholderPageComponent`). The page, dialog, and chip-list described below are the **intended** implementation; the backend tag endpoints and the Angular dialog/chip-list components still ship in source. Treat the steps below as the contract a Playwright test will exercise once the routes are wired up.

## Summary

Tags label notes (and potentially other entities later). Tags are scoped per profile, addressable by slug, and used for the "browse notes by tag" view. Tags are typically created inline as the user is tagging a note (see [`note-management`](../note-management/README.md)) but can also be added explicitly via the add-tag dialog.

## Surface area

- Page: `pages/tags/tags-page/...` (`/tags`)
- Dialog: `components/add-tag-dialog/...`
- Inline picker: `components/auto-complete-chip-list/...`
- Service: `services/tags.service.ts` — endpoints: `GET /api/tags`, `POST /api/tags`, `GET /api/tags/slug/<slug>`, `DELETE /api/tags/<id>`.
- Browse-by-tag: `/notes/by-tag/<slug>` page (see notes flow).

## Preconditions

- Authenticated with active profile.

## Steps

1. **Open Tags.**
   - Navigate to `/tags` (the sidenav has no direct entry; reach via a note edit page or by URL).
   - **Assert:** the page renders the existing tags as a list/grid.

2. **Add a tag explicitly.**
   - Open the add-tag dialog (FAB or inline) → enter a name (e.g. `growth`) → save.
   - **Assert:** dialog closes; the tag appears in the list; `POST /api/tags` returns 2xx with `slug = "growth"` (verify the slug is the kebab-cased name).

3. **Resolve a tag by slug.**
   - Issue `GET /api/tags/slug/growth`.
   - **Assert:** 200 with the matching tag id and name.

4. **Add a tag inline while editing a note.**
   - Go to a note's edit page → in the chip-list input, type `health` and press `Enter`.
   - **Assert:** chip appears; the call to `POST /api/tags` (or chip-list internal flow) succeeds; on save, the note is associated with the tag.

5. **Browse notes by tag.**
   - Navigate to `/notes/by-tag/growth`.
   - **Assert:** all notes tagged `growth` are listed; clicking a row opens its edit page; navigating to a non-existent slug returns an empty state.

6. **Delete a tag with no usage.**
   - From `/tags` (or via API), delete a tag that no note references.
   - **Assert:** `DELETE /api/tags/<id>` returns 2xx; tag disappears from list.

7. **Delete a tag still in use.**
   - Try to delete a tag that at least one note references.
   - **Assert:** the API returns 4xx (or cascades by detaching tags from notes — confirm against `Features/Tag/...` and assert the actual contract).

## Selectors

| Need | Selector |
| --- | --- |
| Add-tag dialog | role `dialog` |
| Tag name input | `getByLabel('Name')` (verify) |
| Tag chip in chip list | `mat-chip` with text |

> **No `data-testid` on the tags page or dialog.** Add `data-testid="tag-row-<id>"`, `data-testid="add-tag-name"`, `data-testid="add-tag-submit"`.

## Edge cases

- Two profiles each create a tag named `health` → each gets its own `id` and slug, scoped by profile.
- Slug collision within a profile → API rejects with 4xx.
- Deleting a tag while a `notes-by-tag/<slug>` page is open in another tab → stale page should re-route or show empty.
