# Notes — Detailed Design

**Status:** Implemented

**Traces to:** L1-007 · L2-014, L2-038, L2-041

## 1. Overview

The Notes page (`pages/notes`, route `/notes`) is the user's catalog of rich-text notes. It lists notes for the active profile (title + last-modified) and provides "New note" and edit-cell actions that navigate to `/edit-note/:slug` (design `13-edit-note`).

**The entire `Note` aggregate is missing on the backend.** This design adds the bounded context (entity + persistence + endpoints) inside the existing `Commitments` module — Notes are profile-scoped just like activities and to-dos, so a new module is unnecessary. The `NoteSavedEvent` integration event already exists in `Commitments.Shared/IntegrationEvents.cs` so the realtime notifier (`NoteTagRealtimeNotifier`) can wire up immediately once the handlers publish it.

## 2. Architecture

### 2.1 C4 Component
![C4 Component](diagrams/c4_component.png)

## 3. Component Details

### 3.1 NotesPageComponent (frontend, exists as placeholder)
- Replace placeholder route with this component.
- Columns: title, lastModifiedOn, edit, delete. FAB → navigates to `/edit-note/new` (no slug yet).
- Loads via `NotesService.getByCurrentUser()` (already calls `GET /api/notes/currentuser` in the service).

### 3.2 Note backend (entirely **new**)
- New domain entity `Commitments.Domain.Note { NoteId : Guid, ProfileId : Guid, Title : string, Slug : string, Body : string (HTML), CreatedOn, LastModifiedOn, IsDeleted }`.
- EF migration: `Notes` table with composite index `(ProfileId, Slug)` unique.
- `Slug` regenerated server-side from `Title` (kebab case, deduplicated by appending `-2`, `-3` if a row with the same `(ProfileId, Slug)` already exists).
- New vertical slices in `Commitments.Features.Note`:
  - `SaveNote` — sanitises `Body` HTML server-side (strip `<script>` and event-handler attributes per L2-041) using HtmlSanitizer or equivalent. Publishes `NoteSavedEvent` to the bus.
  - `RemoveNote` — soft-delete; publishes `NoteRemovedEvent`.
  - `GetNotes` (active profile, ordered by `LastModifiedOn` desc).
  - `GetNoteBySlug` — used by `/edit-note/:slug` (design `13`).
  - `GetNoteByTitleAndCurrentProfile` — used by the existing frontend `getByTitleAndCurrentUser` call.
  - `GetNoteById` — used by the dialog/cell.
- New `NoteController` under `Commitments.Controllers` exposing the routes the frontend already calls (see `notes.service.ts`):
  - `GET /api/notes/currentuser`, `GET /api/notes/slug/{slug}`, `GET /api/notes/getById?id=`, `GET /api/notes/getByTitleAndCurrentUser?title=`, `POST /api/notes`, `DELETE /api/notes/{noteId}`.

## 4. Data Model

### 4.1 Class Diagram
![Class Diagram](diagrams/class_diagram.png)

## 5. Key Workflows

### 5.1 List notes for current profile
![Sequence — List](diagrams/sequence_list.png)

## 6. API Contracts

| Method | Route | Body / Params | Response |
|--------|-------|----------------|----------|
| GET | `/api/v1.0/notes/currentuser` | — | `200 { notes }` (active profile, sorted desc) |
| GET | `/api/v1.0/notes/slug/{slug}` | — | `200 { note }` / `404` |
| GET | `/api/v1.0/notes/getById?id={id}` | `id : Guid` | `200 { note }` / `404` |
| GET | `/api/v1.0/notes/getByTitleAndCurrentUser?title=` | `title : string` | `200 { note }` / `404` |
| POST | `/api/v1.0/notes` | `{ note }` | `200 { noteId, slug }` |
| DELETE | `/api/v1.0/notes/{noteId}` | — | `200` |

## 7. Security Considerations

- All endpoints `[Authorize]`, scoped to `ProfileId` header (L2-038). Cross-profile note reads return 404.
- `Body` is sanitised on **write** (server-side), and rendered with Angular's default property binding on **read** so script tags can never reach the DOM (L2-041 AC #1).
- Slug duplication is resolved server-side; the client never controls slug uniqueness.

## 8. ATDD Slices

1. **Slice A — entity + migration.** Add `Note` to `CommitmentsDbContext`, produce migration, seed nothing. **Status: Implemented** — entity + DbContext entry + query filter; EF migration carried with the 01-Login backlog.
2. **Slice B — list endpoint + page wiring.** **Status: Implemented** — `GetNotesByCurrentUser` filters by `ProfileId` + `OrderByDescending(LastModifiedOn)`, route maps `/notes` to `NotesPageComponent`.
3. **Slice C — save endpoint with sanitisation.** **Status: Implemented** — two regex-pass strip of `<script>` blocks and `on*=` attributes; slug regenerated from `Title` and deduped within profile by appending `-2`, `-3`...
4. **Slice D — delete + soft-delete.** **Status: Implemented** — `RemoveNote` calls `Remove()` and `BaseDbContext.OnSavingChanges` flips to `IsDeleted=true`; `NoteRemovedEvent` published.

## 10. Implementation Notes

- Body sanitisation is **two regexes**, not a heavyweight HtmlSanitizer — strip `<script>…</script>` and any attribute starting with `on…`. A junior dev can read the rules in 30 seconds; if the threat model grows, swap in `HtmlSanitizer` later.
- Slug dedupe is one LINQ projection of taken slugs followed by an `i = 2…` loop. Bounded by collisions in the same profile — fast on typical data.
- `NoteSavedEvent` / `NoteRemovedEvent` already shipped in `Commitments.Shared/IntegrationEvents.cs`; the new handlers publish them so the existing `NoteTagRealtimeNotifier` fans out without further wiring.
- Composite unique `(ProfileId, Slug)` index belongs to the deferred migration; the dedupe loop maintains the invariant in code today.

## 9. Open Questions

- Is the slug user-editable? Recommendation: yes, but only as an optional override; if omitted, the server slugs the title. Out of this slice.
- Real-time updates: `NoteSavedEvent` is already published by the existing notifier — **delta:** publish the event from the new `SaveNoteHandler` so SignalR fans out to the `profile:{profileId}` group.
