# Note management

> **Status (2026-04-27):** the `/notes` route resolves to `PlaceholderPageComponent` ("Coming soon"); the legacy `/notes/create`, `/notes/by-tag/:slug`, and `/notes/edit/:id` routes referenced below are not registered in `app.routes.ts` today. The pages, Quill editor, tag chip-list, and digital-asset input described below are the **intended** implementation; the backend (Notes / Tags / DigitalAssets modules) and the relevant Angular components still ship in source. Treat the steps below as the contract a Playwright test will exercise once the placeholder is replaced and the sub-routes are wired up.

## Summary

The user can create rich-text notes (Quill editor), tag them, list them, browse notes by tag, edit, and delete. Notes are profile-scoped and may attach digital assets via the `digital-asset-url-input` component.

## Surface area

- Pages:
  - `pages/notes/notes-page/...` (`/notes/create` and listing)
  - `pages/notes-by-tag/notes-by-tag-page/...` (`/notes/by-tag/:slug`)
  - `pages/edit-note/edit-note-page/...` (`/notes/edit/:id`)
- Editor: `components/quill-text-editor/...`
- Asset input: `components/digital-asset-url-input/...`
- Tag pickers: `components/auto-complete-chip-list/...`, `components/add-tag-dialog/...`
- Services: `services/notes.service.ts`, `services/tags.service.ts`, `services/digital-asset.service.ts`
- Backend:
  - Notes: `Modules/Commitments/Controllers/...` (Notes feature) — see code under `Features/Note/...`.
  - Tags: `services/tags.service.ts` calls `/api/tags` (see also [`tag-management`](../tag-management/README.md)).
  - Digital assets: `Modules/DigitalAssets/...`.

## Preconditions

- Authenticated with active profile.
- Digital asset uploads work end-to-end (DigitalAssets module reachable).

## Steps

1. **Open the create-note page.**
   - Sidenav → `Notes`.
   - **Assert:** URL `/notes/create`; the Quill editor is visible and focusable; a tag chip-list is visible; a save button is visible (initially disabled or enabled depending on validation).

2. **Author a note.**
   - Type a title (if a title field exists), then type body text in the Quill editor and apply at least one formatting (bold, list, heading).
   - **Assert:** the editor's contenteditable shows the formatted content (e.g. `<strong>` markup).

3. **Attach a digital asset.**
   - In `digital-asset-url-input`, paste/upload an asset URL.
   - **Assert:** preview renders; an asset id is captured by the form.

4. **Add tags.**
   - Type a new tag name into the chip list, press `Enter`.
   - **Assert:** a chip with that label appears; the tag is created in the backend (`POST /api/tags` returns 2xx) on first use of a previously-unknown tag.

5. **Save the note.**
   - Click `Save`.
   - **Assert:** the app navigates to `/notes/edit/<newId>` (or back to the listing); `POST /api/notes` returns 201; the new note id is in the response.

6. **Edit the note.**
   - From listing, click a row → `/notes/edit/<id>` loads.
   - **Assert:** Quill editor pre-populated with stored body; tag chips reflect saved tags.
   - Make a change and click `Save`.
   - **Assert:** `PUT /api/notes/<id>` returns 2xx; reload shows the change persisted.

7. **Browse notes by tag.**
   - Navigate to `/notes/by-tag/<slug>` (slug from a known tag).
   - **Assert:** only notes tagged with that tag are listed; clicking a note opens its edit page.

8. **Delete a note.**
   - From listing, delete a note → confirm.
   - **Assert:** row removed; `DELETE /api/notes/<id>` returns 2xx; visiting `/notes/edit/<id>` returns 4xx / not found.

## Selectors

| Need | Selector |
| --- | --- |
| Quill editor | `.ql-editor` |
| Tag chip list input | `mat-chip-grid input` (or the `auto-complete-chip-list` component's input) |
| Save button | `getByRole('button', { name: /save/i })` |
| Notes sidenav link | `getByRole('button', { name: /notes/i })` |

> **Add `data-testid="note-title"`, `data-testid="note-save"`, `data-testid="note-row-<id>"`, `data-testid="quill-editor"`** when authoring.

## Edge cases

- Saving with empty body → validator rejects.
- Adding a tag whose slug collides with an existing tag → existing tag re-used (no duplicate).
- Removing the last tag from a note → `notes-by-tag` for that slug should no longer list it.
- Quill autosave (if implemented) — flow doc must be updated when added.
