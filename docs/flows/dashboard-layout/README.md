# Dashboard layout

## Summary

The dashboard renders a grid of tiles. Each user has a personalised layout: which tiles, their order, and their persisted state in `localStorage`. The user can add tiles from a catalog, remove tiles via edit-mode chrome, reset to the default layout, and reload without losing customisation.

This flow is already covered end-to-end by `frontend/projects/commitments-app/e2e/dashboard.spec.ts` — use that as a reference test. This document is the canonical description of the user-visible behaviour the test guards.

## Surface area

- Shell: `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.{ts,html}`
- Grid: `commitments-dashboard-grid` component (rendered inside the shell)
- Layout store: `LAYOUT_STORAGE_KEY = 'commitments.dashboard.layout.v1'` (`projects/commitments-app/e2e/pages/dashboard.page.ts`)
- Default tiles: `Daily Results`, `Weekly Focus`, `Monthly Progress`, `Outstanding To Dos`, `Relations` (`DEFAULT_TILE_TITLES`)
- Plugin tiles: `Consistency Trend`, `Live Goal Metrics`, `Review Goal History`

## Preconditions

- Authenticated, dashboard reachable at `/`.
- Test should clear `localStorage[LAYOUT_STORAGE_KEY]` in `beforeEach` (the page object already does this).

## Steps

1. **Default dashboard renders.**
   - Navigate to `/`.
   - **Assert:** `dashboard-shell` is visible; `dashboard-grid` is visible; exactly 5 tiles visible (`Daily Results`, `Weekly Focus`, `Monthly Progress`, `Outstanding To Dos`, `Relations`).

2. **Catalog reflects the active mode.**
   - In live mode, open the `tile-select` dropdown.
   - **Assert:** options are exactly: `Daily Results`, `Weekly Focus`, `Monthly Progress`, `Outstanding To Dos`, `Relations`, `Consistency Trend`, `Live Goal Metrics` (review-only tiles like `Review Goal History` are filtered out).

3. **Add a tile.**
   - Select `Monthly Progress` → click `Add Tile`.
   - **Assert:** tile count increments to 6; a new `tile-shell` titled `Monthly Progress` is visible (alongside the original); `localStorage[LAYOUT_STORAGE_KEY].items.length === 6`.

4. **Layout persists across reload.**
   - Reload the page without clearing `localStorage`.
   - **Assert:** tile count remains 6; the duplicate `Monthly Progress` is still present.

5. **Enter edit mode reveals tile chrome.**
   - Click `Edit Layout`.
   - **Assert:** button text changes to `Done`; each tile has a remove button (`remove-tile`); count of remove buttons equals tile count.

6. **Remove a tile.**
   - In edit mode, click the first remove-tile button.
   - **Assert:** tile count decrements; `localStorage` count updates accordingly.

7. **Exit edit mode.**
   - Click `Done`.
   - **Assert:** button text returns to `Edit Layout`; remove buttons are gone.

8. **Reset to default layout.**
   - In edit mode, remove a tile → click `Reset` → exit edit mode.
   - **Assert:** the dashboard returns to the 5 default tiles.

## Selectors

All stable `data-testid` values exist in the shell:

| Need | Selector |
| --- | --- |
| Dashboard shell | `getByTestId('dashboard-shell')` |
| Grid | `getByTestId('dashboard-grid')` |
| Tile (each) | `getByTestId('dashboard-tile')` |
| Tile shell (each) | `getByTestId('tile-shell')` |
| Tile title | `getByTestId('tile-title')` |
| Tile catalog `<select>` | `getByTestId('tile-select')` |
| Add Tile button | `getByTestId('add-tile')` |
| Edit / Done button | `getByTestId('edit-layout')` |
| Reset button | `getByTestId('reset-layout')` |
| Per-tile remove button | `getByTestId('remove-tile')` |

## Edge cases

- Layout `localStorage` is corrupted → shell should fall back to default and overwrite.
- Adding the same tile twice (e.g. two `Consistency Trend` tiles) → both render and persist.
- Reordering tiles (gridster drag) — not currently asserted in the existing spec; if added, document here.
- Layout migrations: storage key is versioned (`.v1`); a future schema bump should drop and rebuild.
