# Frequency catalog management

> **Status (2026-04-27):** the `/frequencies` route resolves to `PlaceholderPageComponent` ("Coming soon"); the dedicated `/frequencies/edit/:id` route is not in `app.routes.ts` today. The catalog page, edit dialog, dedicated edit page, and ag-grid described below are the **intended** implementation; the backend controllers, MediatR features, and dialog component still ship in source. Treat the steps below as the contract a Playwright test will exercise once the placeholder is replaced.

## Summary

A user maintains a catalog of **frequencies** (e.g. "1 per day", "3 per week") classified by **frequency type** (e.g. "per day", "per week", "per month", "per 48 hours / per 72 after 3 occurrences"). Frequencies are referenced by commitments to express how often a behaviour is expected. Frequency types are seeded by `SeedData.cs`.

## Surface area

- Pages:
  - `pages/frequencies/frequencies-page/...` (`/frequencies`)
  - `pages/edit-frequency/edit-frequency-page/...` (`/frequencies/edit/:id`)
- Dialog: `components/edit-frequency-dialog/...`
- Editor: `components/frequency-editor/...`, `components/frequencies-editor/...`
- Services: `services/frequency.service.ts`, `services/frequency-type.service.ts`
- Backend: `Modules/Commitments/Controllers/FrequencyController.cs`, `FrequencyTypeController.cs` and `Features/Frequency*`.

## Preconditions

- Authenticated, with an active profile.
- Frequency types are seeded.

## Steps

1. **Open Frequencies.**
   - Sidenav → `Frequencies`.
   - **Assert:** URL `/frequencies`; heading `Frequencies`; ag-grid present.

2. **Create a frequency.**
   - Click FAB → enter `Quantity` (integer ≥ 1, e.g. 2) and pick `Frequency Type` (e.g. `per day`).
   - Submit.
   - **Assert:** dialog closes; new row appears showing the chosen quantity + type (e.g. "2 per day").

3. **Edit a frequency from the grid.**
   - Open an edit affordance on a row → modify quantity → save.
   - **Assert:** grid row updates; `PUT /api/frequencies/<id>` returns 2xx.

4. **Edit a frequency on its dedicated page.**
   - Navigate to `/frequencies/edit/<id>`.
   - **Assert:** the frequency editor renders pre-populated; saving navigates back to `/frequencies` with updated row.

5. **Delete a frequency not referenced by any commitment.**
   - Click delete on a row → confirm.
   - **Assert:** row removed; `DELETE /api/frequencies/<id>` returns 2xx.

6. **Delete a frequency referenced by a commitment is rejected.**
   - Try to delete a frequency in use.
   - **Assert:** error surface; row remains.

## Selectors

| Need | Selector |
| --- | --- |
| Frequencies FAB | first `mat-fab` on `/frequencies` |
| Quantity input | `getByLabel('Quantity')` |
| Frequency type select | role `combobox` named `Frequency Type` |

> **Add `data-testid="frequency-row-<id>"` and `data-testid="frequency-quantity-input"`** when authoring.

## Edge cases

- Quantity 0 or negative → blocked by validator.
- Duplicate (Quantity + Type) pair → API/validator rejects.
- `frequencies-editor` is also embedded inside the commitment edit dialog (see [`commitment-tracking`](../commitment-tracking/README.md)).
