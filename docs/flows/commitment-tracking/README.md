# Commitment tracking

> **Status (2026-04-27):** the `/commitments` route is wired in `app.routes.ts` to `PlaceholderPageComponent` ("Coming soon"). The catalog page, edit dialog, FAB, and ag-grid described below are the **intended** implementation; the backend controller, MediatR features, and dialog component all still ship in source. Treat the steps below as the contract a Playwright test will exercise once the placeholder is replaced.

## Summary

A user defines **commitments** that pair a behaviour with one or more frequencies (and optional pre-conditions). The commitment is the unit the dashboard tiles surface live and review-mode metrics for. The user can list commitments, create one, edit one, and remove one.

## Surface area

- Page: `pages/commitments/commitments-page/...` (`/commitments`)
- Dialog: `components/edit-commitment-dialog/...`
- Embedded editor: `components/frequencies-editor/...` (lets the user attach multiple frequencies in one dialog).
- Service: `services/commitment.service.ts`
- Backend: `Modules/Commitments/Controllers/CommitmentController.cs`, MediatR features under `Modules/Commitments/Features/Commitment*`.
- Domain: `Commitment` aggregate (`Modules/Commitments/Domain/CommitmentAggregate/...`) — tracks `BehaviourId`, frequencies, profile.
- Realtime: when a commitment fires `goalProgressUpdated`, dashboard tiles for that goal refresh via SignalR (`HubClient` → `commitments/realtime`).

## Preconditions

- Authenticated, with an active profile.
- At least one behaviour and one frequency exist (see [`behaviour-management`](../behaviour-management/README.md), [`frequency-management`](../frequency-management/README.md)).

## Steps

1. **Open Commitments.**
   - Sidenav → `Commitments`.
   - **Assert:** URL `/commitments`; heading `Commitments`; ag-grid present, paginated to 5 rows.

2. **Create a commitment.**
   - Click FAB → in the dialog, pick a behaviour, attach one or more frequencies via the frequencies editor, optionally add a description.
   - Submit.
   - **Assert:** dialog closes; new row in the grid showing the behaviour name and a summary of attached frequencies.

3. **Edit a commitment.**
   - Open a row's edit affordance, change behaviour or attached frequencies, save.
   - **Assert:** grid row updates; `PUT /api/commitments/<id>` returns 2xx.

4. **Add a second frequency to an existing commitment.**
   - Edit a row → in the frequencies editor, add another frequency (e.g. `1 per week`) → save.
   - **Assert:** the commitment row's frequency summary now includes both.

5. **Delete a commitment.**
   - Click delete on a row → confirm.
   - **Assert:** row removed; `DELETE /api/commitments/<id>` returns 2xx; any dashboard tile bound to that goal stops updating (or shows an empty state).

6. **Pagination works.**
   - With more than 5 commitments, click the next-page arrow.
   - **Assert:** different rows render; total count chip matches the number of commitments returned by `GET /api/commitments`.

7. **Realtime: progress updates push to the dashboard.**
   - Open `/` (dashboard) in tab A and `/commitments` in tab B.
   - In tab B, record an activity (see [`activity-recording`](../activity-recording/README.md)) for the commitment's behaviour.
   - **Assert:** in tab A, the live tile for that commitment updates without a reload (SignalR `goalProgressUpdated`).

## Selectors

| Need | Selector |
| --- | --- |
| Commitments FAB | first `mat-fab` on `/commitments` |
| Behaviour select in dialog | role `combobox` named `Behaviour` |
| Frequencies editor add button | `getByRole('button', { name: /add frequency/i })` (verify exact text) |
| Grid row | `.ag-row[role="row"]` filtered by behaviour-name cell text |

> **Add `data-testid="commitment-row-<id>"`, `data-testid="commitment-save"`, `data-testid="frequencies-editor-add"`** when authoring.

## Edge cases

- Commitment with zero frequencies → validator rejects.
- Commitment whose behaviour was deleted in another session → dialog refetch should drop or warn.
- Realtime path: ensure `HubClient.messages$` is subscribed before recording the activity, otherwise the assertion can race.
