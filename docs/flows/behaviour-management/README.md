# Behaviour catalog management

## Summary

A user maintains a catalog of **behaviours** (e.g. "Drink 2L water") classified by **behaviour type** (e.g. "Health"). Behaviours are the building blocks for commitments and activities. The user can list, create, edit, and delete behaviours, plus list and create behaviour types. Behaviour Types `Health`, `Acts Of Service`, `Physical Touch`, `Gifts`, `Words of Affirmation`, `Quality Time`, `Respect`, `Listening` are seeded by `SeedData.cs`.

## Surface area

- Pages:
  - `pages/behaviours/behaviours-page/...` (`/behaviours`)
  - `pages/behaviour-types/behaviour-types-page/...` (`/behaviour-types`)
- Dialogs:
  - `components/edit-behaviour-dialog/...`
  - `components/edit-behaviour-type-dialog/...`
- Services: `services/behaviour.service.ts`, `services/behaviour-type.service.ts`
- Backend: `Modules/Commitments/Controllers/BehaviourController.cs`, `BehaviourTypeController.cs`, MediatR features under `Modules/Commitments/Features/Behaviour*` and `BehaviourType*`.
- Cross-cutting: every read/write is profile-scoped via `ProfileId` header.

## Preconditions

- Authenticated, with an active profile.
- Behaviour types are seeded (`migratedb seeddb`).

## Steps

### Behaviour Types

1. **Open Behaviour Types.**
   - Sidenav → `Behaviour Types`.
   - **Assert:** URL `/behaviour-types`; page heading `Behaviour Types`; ag-grid renders with at least the seeded types (`Health`, `Acts Of Service`, ...).

2. **Create a behaviour type.**
   - Click the FAB.
   - In the dialog, enter Name (e.g. `Sleep`), submit.
   - **Assert:** dialog closes; new row appears in the grid with the entered name.

3. **Edit a behaviour type.**
   - Click the edit affordance on a row (or click the row).
   - Change the name, save.
   - **Assert:** grid row reflects the new name; `PUT /api/behaviourTypes/<id>` returns 2xx.

4. **Delete a behaviour type.**
   - Click the delete affordance on a row not referenced by any behaviour.
   - Confirm.
   - **Assert:** row removed; `DELETE /api/behaviourTypes/<id>` returns 2xx.

### Behaviours

5. **Open Behaviours.**
   - Sidenav → `Behaviours`.
   - **Assert:** URL `/behaviours`; heading `Behaviours`; ag-grid present.

6. **Create a behaviour.**
   - Click the FAB.
   - In the dialog, enter Name (e.g. `Drink 2L water`) and pick a behaviour type from the type select.
   - Submit.
   - **Assert:** dialog closes; new row in the grid; row's behaviour type column matches the selected type.

7. **Edit a behaviour.**
   - Open a row's edit dialog, change the name, save.
   - **Assert:** grid row updates; `PUT /api/behaviours/<id>` returns 2xx.

8. **Delete a behaviour not used by any commitment.**
   - Trigger row delete; confirm.
   - **Assert:** row removed; `DELETE /api/behaviours/<id>` returns 2xx.

9. **Delete a behaviour referenced by a commitment is rejected.**
   - Try to delete a behaviour that a commitment depends on.
   - **Assert:** API returns 4xx; row remains; user-visible error.

## Selectors

| Need | Selector |
| --- | --- |
| Behaviours FAB | first `mat-fab` on `/behaviours` |
| Behaviour Types FAB | first `mat-fab` on `/behaviour-types` |
| Edit dialog name input | role `textbox` named `Name` (label/placeholder) |
| Type select in behaviour dialog | role `combobox` with name `Behaviour Type` |
| Grid row | `.ag-row[role="row"]` (filter by cell text) |

> Behaviours/Behaviour Types pages have **no `data-testid` coverage**. Add `data-testid="page-fab"`, `data-testid="behaviour-row-<id>"`, `data-testid="behaviour-name-input"` etc. when authoring.

## Edge cases

- Duplicate name → API/validator rejects; dialog stays open with error.
- Empty name → FluentValidation `.NotEmpty()` rejects (`backend/src/Modules/Commitments/Features/Behaviour/...`).
- Selecting a type that was just deleted in another tab → dialog should refetch type list or show a stale-data error.
