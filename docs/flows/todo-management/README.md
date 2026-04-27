# To-Do management

> **Status (2026-04-27):** the `/to-dos` route resolves to `PlaceholderPageComponent` ("Coming soon"). The catalog page, edit dialog, and ag-grid described below are the **intended** implementation; the backend controller, MediatR features, and dialog component still ship in source. Treat the steps below as the contract a Playwright test will exercise once the placeholder is replaced.

## Summary

The user keeps a list of to-do items. Outstanding (incomplete) to-dos are surfaced on the dashboard's `Outstanding To Dos` tile and via `GET /api/toDos/outstanding`. The user can create, edit, complete, and delete items.

## Surface area

- Page: `pages/to-dos/to-dos-page/...` (`/to-dos`)
- Dialog: `components/edit-to-do-dialog/...`
- Service: `services/to-do.service.ts`
- Backend: `Modules/Commitments/Controllers/...` (ToDos feature), `Features/ToDo/...`
- Dashboard tile: the catalog `Outstanding To Dos` tile reads outstanding items.

## Preconditions

- Authenticated with active profile.

## Steps

1. **Open To-Dos.**
   - Sidenav → `To Do's`.
   - **Assert:** URL `/to-dos`; heading `To Do's`; ag-grid renders, paginated to 5.

2. **Create a to-do.**
   - Click FAB → in the dialog, enter a title and optional notes/due-date → save.
   - **Assert:** dialog closes; new row in the grid with `Completed: false`; `POST /api/toDos` returns 201.

3. **Edit a to-do.**
   - Open a row's edit affordance → change the title → save.
   - **Assert:** row updates; `PUT /api/toDos/<id>` returns 2xx.

4. **Mark a to-do complete.**
   - Click the row's complete checkbox (`checkbox-cell` component).
   - **Assert:** row state flips to completed (visual style change, `Completed` column shows `true`); `PUT /api/toDos/<id>` returns 2xx; the dashboard's `Outstanding To Dos` tile decreases by one if open in another tab.

5. **Mark a to-do incomplete (toggle back).**
   - Click the same checkbox again.
   - **Assert:** row state toggles back; `Outstanding To Dos` count returns to its prior value.

6. **Delete a to-do.**
   - Click delete on a row → confirm.
   - **Assert:** row removed; `DELETE /api/toDos/<id>` returns 2xx.

7. **Outstanding endpoint excludes completed.**
   - Hit `GET /api/toDos/outstanding` directly.
   - **Assert:** the response excludes any rows whose `Completed` is `true`.

## Selectors

| Need | Selector |
| --- | --- |
| To-Dos FAB | first `mat-fab` on `/to-dos` |
| Title input in dialog | `getByLabel('Title')` |
| Complete checkbox in row | the `checkbox-cell` component's checkbox in the row |

> **Add `data-testid="todo-row-<id>"`, `data-testid="todo-complete-<id>"`, `data-testid="todo-save"`** when authoring.

## Edge cases

- Toggle complete on a row, refresh page → state persists.
- Empty title → validator rejects.
- Many to-dos → pagination chip count matches API total.
- Realtime: completing a to-do should drop the dashboard `Outstanding To Dos` count via SignalR (verify only if the realtime path is implemented for to-dos; today the hub publishes `goalProgressUpdated` and `dashboardTileInvalidated`).
