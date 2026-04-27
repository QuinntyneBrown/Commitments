# Activity recording

## Summary

An **activity** is one instance of a behaviour being performed at a point in time. Recording an activity is what drives commitment progress: dashboard tiles, the `last14` endpoint, and the `trend` endpoint all aggregate over the activities table. The user can record an activity, browse activity history, edit, and delete entries.

## Surface area

- Page: `pages/activities/activities-page/...` (`/activities`)
- Dialog: `components/edit-activity-dialog/...`
- Service: `services/activity.service.ts`
- Backend:
  - Controller: `Modules/Commitments/Controllers/ActivityController.cs`
  - MediatR features: `Modules/Commitments/Features/Activity/...`
  - Trend / progress projections: `Features/GoalProgress/GetGoalTrend.cs`, `GetGoalProgressLast14.cs` group activities by `PerformedOn.Date`.
- Domain: `Activity` aggregate has `BehaviourId`, `ProfileId`, `PerformedOn`.
- Realtime: an activity that affects an active commitment publishes `goalProgressUpdated` (see `Modules/Commitments/.../Realtime/...`).

## Preconditions

- Authenticated, with an active profile.
- At least one behaviour exists.
- For realtime asserts: at least one commitment exists targeting the chosen behaviour.

## Steps

1. **Open Activities.**
   - Sidenav → `Activities`.
   - **Assert:** URL `/activities`; heading `Activities`; ag-grid present, paginated to 5 rows.

2. **Record an activity for today.**
   - Click FAB → in the dialog, select a behaviour, set `Performed On` to today, save.
   - **Assert:** dialog closes; the new row appears in the grid; `POST /api/activities` returns 201.

3. **Record a back-dated activity.**
   - Click FAB → pick a behaviour and a `Performed On` date 5 days ago, save.
   - **Assert:** new row visible with the back-dated `PerformedOn`; the dashboard's `Consistency Trend` for the matching goal shows a non-zero `completed` count on that day (verify via `GET /api/v1.0/goal-progress/trend`).

4. **Edit an activity.**
   - Open the edit affordance on a row → change the date or behaviour → save.
   - **Assert:** row updates; `PUT /api/activities/<id>` returns 2xx.

5. **Delete an activity.**
   - Click delete on a row → confirm.
   - **Assert:** row removed; `DELETE /api/activities/<id>` returns 2xx.

6. **Activity history pagination + sort.**
   - With > 5 activities, page through the grid.
   - **Assert:** rows differ per page; default sort is by `PerformedOn` desc (verify by row order).

7. **Realtime push to dashboard tiles.**
   - In tab A open `/` (a dashboard with a `Live Goal Metrics` and `Consistency Trend` tile bound to the goal whose behaviour you'll record).
   - In tab B record an activity for that behaviour today.
   - **Assert:** in tab A, the live metric count increments and the chart's most recent point's percentage rises — without page reload.

## Selectors

| Need | Selector |
| --- | --- |
| Activities FAB | first `mat-fab` on `/activities` |
| Behaviour select in activity dialog | role `combobox` named `Behaviour` |
| Performed On date input | `getByLabel('Performed On')` (or matching label text) |

> **Add `data-testid="activity-row-<id>"`, `data-testid="activity-save"`, `data-testid="activity-performed-on"`** when authoring.

## Edge cases

- Future-dated activity → API trims `asOf` to `UtcNow` in trend handler; UI should still allow saving but flag the date.
- Deleting an activity reduces the trend `completed` for that day — assert the chart point updates if the dashboard is open.
- Activity for a behaviour not used by any commitment → still recorded but no live tile fires.
