# Dashboard layout

## Summary

The dashboard renders a grid of tiles. Each user has a personalised layout: which tiles, their order, and their persisted state in `localStorage`. The user can add tiles from a modal catalog (FAB → Add Tile dialog), remove tiles via edit-mode chrome, and reload without losing customisation. Layout state is persisted **per mode** — live and review have independent layouts.

This flow has partial Playwright coverage in `frontend/projects/commitments-app/e2e/dashboard.spec.ts`. **Note:** the existing spec and its `DashboardPage` page-object still target the pre-redesign UI (inline `tile-select`, `add-tile`, `edit-layout`, `reset-layout` buttons). They need to be re-authored against the new FAB + dialog + edit-mode-toggle surface described below — this document is the canonical description of the current UI.

## Surface area

- App shell: `frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.{ts,html,scss}` (top toolbar + left sidenav + router outlet — wraps every authenticated route).
- Dashboard shell: `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.{ts,html,scss}` (PrimaryHeader + grid + FAB).
- Grid: `commitments-dashboard-grid` (`frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.{ts,html}`).
- Add Tile dialog: `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.{ts,html,scss}`.
- Layout store: `DashboardLayoutStore` (`dashboard-framework/src/lib/dashboard/dashboard-layout.store.ts`) — owns `liveLayout`, `reviewLayout`, `isEditMode`, `addTile`, `removeTile`, `resetLayout`.
- Persistence: `LayoutPersistenceService` (`layout-persistence.service.ts`) writes to two `localStorage` keys, **one per mode**:
  - `commitments.layout.live` → `LIVE_LAYOUT_STORAGE_KEY`
  - `commitments.layout.review` → `REVIEW_LAYOUT_STORAGE_KEY`
  - Both store `{ schemaVersion: 1, savedAt: number, items: DashboardItem[] }`.
- Default tiles seeded by tile-registry (see each tile's `includeByDefault` flag): `Daily Results`, `Weekly Focus`, `Monthly Progress`, `Outstanding To‑Dos`, `Relations`.
- Plugin tile catalog: `Consistency Trend`, `Live Goal Metrics` (live + review), `Review Goal History` (review-only).

## Preconditions

- Authenticated, dashboard reachable at `/`.
- Test should clear `localStorage[commitments.layout.live]` (and `commitments.layout.review` if exercising review) in `beforeEach`.

## Steps

1. **Default dashboard renders.**
   - Navigate to `/`.
   - **Assert:** `dashboard-layout` is visible (top toolbar with hamburger + brand + profile name + avatar; left sidenav). Inside the content area, `dashboard-shell` is visible; `dashboard-grid` is visible; exactly 5 tiles visible (`Daily Results`, `Weekly Focus`, `Monthly Progress`, `Outstanding To‑Dos`, `Relations`).

2. **Open the Add Tile dialog.**
   - Click the bottom-right FAB (`add-tile-fab`).
   - **Assert:** an `add-tile-dialog` modal opens; its grid lists the registered tiles for the current mode (in live: `Daily Results`, `Weekly Focus`, `Monthly Progress`, `Outstanding To‑Dos`, `Relations`, `Consistency Trend`, `Live Goal Metrics` — `Review Goal History` is filtered out because its `supportedModes` is review-only). The `ADD TILE` button is disabled until a cell is selected.

3. **Add a tile.**
   - Click the `Monthly Progress` cell (`add-tile-cell-monthly-progress`) → click `ADD TILE` (`add-tile-dialog-confirm`).
   - **Assert:** dialog closes; tile count increments to 6; a new `tile-shell` titled `Monthly Progress` is visible (alongside the original); `localStorage['commitments.layout.live'].items.length === 6`.

4. **Cancel discards.**
   - Open the FAB again → click `Daily Results` cell → click `CANCEL` (`add-tile-dialog-cancel`).
   - **Assert:** dialog closes; tile count is unchanged. Same behaviour for the close icon (`add-tile-dialog-close`).

5. **Layout persists across reload.**
   - Reload the page without clearing `localStorage`.
   - **Assert:** tile count remains 6; the duplicate `Monthly Progress` is still present.

6. **Enter edit mode.**
   - Click the `edit` icon-button on the right of the PrimaryHeader (`edit-mode-enter`). It only renders when `mode === 'live'` and `isEditMode === false`.
   - **Assert:** the PrimaryHeader background flips to the accent-strong (pink) colour; an `EDIT MODE` pill (`edit-mode-pill`) renders next to the title; the mode-toggle is hidden; a `DONE` button (`edit-mode-done`) appears in the header actions; each tile has a remove button (`remove-tile`); count of remove buttons equals tile count; the gridster grid is now interactive (drag/resize enabled).

7. **Remove a tile.**
   - In edit mode, click the first remove-tile button.
   - **Assert:** tile count decrements; `localStorage['commitments.layout.live'].items` count updates accordingly.

8. **Exit edit mode.**
   - Click `DONE` (`edit-mode-done`).
   - **Assert:** PrimaryHeader returns to the primary-dim (indigo) colour; `EDIT MODE` pill is gone; mode-toggle reappears; `edit-mode-enter` button reappears; remove buttons are gone; gridster drag/resize is disabled.

9. **Add Tile FAB stays visible in edit mode.**
   - From a clean dashboard, enter edit mode → observe the FAB.
   - **Assert:** the FAB is still rendered (matches design frames `fJpM0`/`a2Cjz`); clicking it opens the same Add Tile dialog. The FAB only hides when the dashboard is in `review` mode (see [`dashboard-modes`](../dashboard-modes/README.md)).

10. **Reset to default layout (programmatic).**
    - There is no `Reset` button in the current UI. `DashboardLayoutStore.resetLayout()` still exists for programmatic use; if the design later surfaces a Reset affordance, document it here.

## Selectors

| Need | Selector |
| --- | --- |
| App-shell layout root | `getByTestId('dashboard-layout')` |
| App-shell toolbar | `getByTestId('dashboard-layout-toolbar')` |
| Hamburger | `getByTestId('dashboard-layout-hamburger')` |
| Profile name | `getByTestId('dashboard-layout-profile-name')` |
| Profile avatar | `getByTestId('dashboard-layout-avatar')` |
| Sidenav | `getByTestId('dashboard-sidenav')` |
| Sidenav item | `getByTestId('sidenav-item-<label>')` (e.g. `sidenav-item-Dashboard`) |
| Dashboard shell | `getByTestId('dashboard-shell')` |
| Grid | `getByTestId('dashboard-grid')` |
| Empty state | `getByTestId('dashboard-empty-state')` |
| Tile (each, gridster cell) | `getByTestId('dashboard-tile')` |
| Tile shell (UI card) | `getByTestId('tile-shell')` |
| Tile title | `getByTestId('tile-title')` |
| Add Tile FAB | `getByTestId('add-tile-fab')` |
| Add Tile dialog root | `getByTestId('add-tile-dialog')` |
| Add Tile dialog close icon | `getByTestId('add-tile-dialog-close')` |
| Add Tile dialog cancel | `getByTestId('add-tile-dialog-cancel')` |
| Add Tile dialog confirm | `getByTestId('add-tile-dialog-confirm')` |
| Add Tile cell (each) | `getByTestId('add-tile-cell-<tileId>')` (e.g. `add-tile-cell-monthly-progress`) |
| Edit-mode enter | `getByTestId('edit-mode-enter')` |
| Edit-mode pill | `getByTestId('edit-mode-pill')` |
| Edit-mode done | `getByTestId('edit-mode-done')` |
| Per-tile remove button | `getByTestId('remove-tile')` |

> Removed in the redesign — do **not** rely on these any more: `getByTestId('tile-select')`, `getByTestId('add-tile')`, `getByTestId('edit-layout')`, `getByTestId('reset-layout')`, `.dashboard-shell__topbar`. The `LAYOUT_STORAGE_KEY = 'commitments.dashboard.layout.v1'` constant is still exported but unused — read/write `commitments.layout.live` / `commitments.layout.review` instead.

## Edge cases

- Layout `localStorage` is corrupted → `LayoutPersistenceService.load()` returns `null` and the store falls back to default and overwrites on the next mutation.
- Adding the same tile twice (e.g. two `Consistency Trend` tiles) → both render and persist (each gets its own `instanceId`).
- Reordering / resizing tiles in edit mode → `itemChangeCallback` writes the new `cols/rows/x/y` back to the store and persists.
- Switching mode preserves both layouts independently (the live layout is restored when the user returns from review).
- Layout migrations: each layout entry is `{ schemaVersion: 1, … }`. A future schema bump should drop and rebuild.
