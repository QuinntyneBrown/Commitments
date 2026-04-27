# 03 — Add Tile flow: replace inline picker with FAB + Dialog

**Status: ACCEPTED**

> **2026-04-27 update**: the design now ships an explicit "Add Tile Dialog" frame (`a2Cjz`) and a dedicated dialog frame (`AhkGr`). The old "Add Dashboard Cards" search-and-chip layout has been **superseded** by a tile-grid layout. This task has been rewritten to match.

**Design** — frames `a2Cjz` (full screen w/ dialog open) and `AhkGr` (dialog itself):
- **FAB** (`FAB/Accent` `e9DYo`, 56×56, `$accent` #FF4081, `$r-full`, drop-shadow blur 10 y6 #00000099, white `add` icon 28×28) is fixed at bottom-right (`bottom: 20px; right: 20px`) on every dashboard variant frame, **including Edit Mode** (`fJpM0`).
- **Backdrop**: full-viewport `#000000B3` overlay with `background_blur` radius **6**.
- **Dialog frame** (`AhkGr`): width **560**, height **620**, `$surface` (#1E1E1E) background, `$r-md` (8) corner, 1px `$divider` stroke, drop-shadow blur 48 y24 `#000000DD`. Vertical layout. Centred on screen (x:360 y:140 within the 1280×900 frame).
- **Header row** (`SHVwp`, padding `[24, 24, 16, 24]`, justify space-between):
  - Title group (vertical, gap 4): "Add Tile" Inter / `$fs-h3` (24) / `$fw-medium` / `$text-primary`; subtitle "Choose a tile to add to your dashboard" Inter / `$fs-body` (14) / normal / `$text-secondary`.
  - Close button (right): 36×36, `$surface-3` fill, `$r-full`, centred `close` icon 18×18 `$text-primary`.
- **Divider** below header: 1px `$divider`, full-width.
- **Body** (`2THAF` "gridWrap", vertical layout, padding `[20, 24]`, gap 12, `fill_container` height): a 2-row × 4-column tile-grid. Each row is a horizontal frame with gap 12 and height **120**. Each tile cell:
  - Default (unselected): `$surface-2` fill, `$r-md` radius, 1px `$divider` stroke, padding 12, vertical layout, centred (justify+align), gap 10.
    - Top: 40×40 circular `$primary-dim` icon container with a 22×22 Material Symbols Rounded icon `$text-primary`.
    - Bottom: tile label Inter / `$fs-sm` (12) / `$fw-medium` / `$text-primary`.
  - Selected: same dimensions, but `$accent` fill (replaces `$surface-2`), 2px `$accent` stroke, drop-shadow blur 12 y4 `#FF408155`. Icon container becomes `#FFFFFF33`, icon goes `$on-accent`, label Inter / `$fs-sm` / `$fw-bold` / `$on-accent`. Add a 18×18 circular `$on-accent` check badge absolutely positioned at top-right (offset x:88 y:6) with a 12×12 `check` icon `$accent`.
  - The reference frame seeds 8 tiles with these icons and labels: `today` Daily Results, `date_range` Weekly Focus, `calendar_month` Monthly Progress, `checklist` To-Dos, `diversity_3` Relations, `image` Poster, `local_fire_department` Streak (selected example), `sticky_note_2` Notes. Use the actual `TileRegistryService` registrations at runtime; the design is illustrative, not normative on tile identity.
- **Divider** above actions: 1px `$divider`, full-width.
- **Action row** (`c9ljk`, padding `[16, 24]`, gap 8, justify end, align centre): two buttons only:
  - `Btn/Basic` "CANCEL" (`CHuxA`).
  - `Btn/Raised/Primary` "ADD TILE" (`AgzGC`).
  - **There is no DELETE, no SAVE, and no search/chip strip.**

**Implementation**: There is no FAB and no dialog. `dashboard-shell` uses an inline native `<select>` + button to add tiles directly. The legacy `add-dashboard-cards-dialog.component.*` and `master-page.component.*` exist but are not on the active route (the active route is `DashboardShellComponent`).

## Tasks
- [ ] **3.1** Add a FAB to `DashboardShellComponent` (or to the dashboard grid frame), fixed at `bottom: 20px; right: 20px; z-index: 2`, 56×56 circular, background `var(--cui-accent)`, `border-radius: var(--cui-radius-full)`, drop-shadow `0 6px 10px #00000099`. Material Symbols Rounded `add` icon, 28×28, `var(--cui-text-on-primary)` (white).
- [ ] **3.2** FAB renders when `modeService.mode() === 'live'` **or** when edit mode is active (see [11-edit-mode.md](11-edit-mode.md)). It does **not** render in review mode (matches `Dashboard Review` frame `TFRTa`).
- [ ] **3.3** Wire the FAB click to open an Add Tile dialog. Create a new `AddTileDialogComponent` under `dashboard-framework/src/lib/dashboard/add-tile-dialog/`. The dialog must list registered tiles from `TileRegistryService.tilesForMode('live')` rendered as a grid of selectable cells, allow exactly one selection (not multi), and on `ADD TILE` call `layoutStore.addTile(tileId)` once for the selected tile. Delete the legacy `add-dashboard-cards-dialog.component.*` files; the search/chip flow they implement is no longer in the design.
- [ ] **3.4** Dialog frame: width **560**, height **620** (use `auto-grow` for short tile counts; clip overflow with internal scroll on the gridWrap row container), background `var(--cui-surface)`, `border-radius: var(--cui-radius-md)`, `border: 1px solid var(--cui-divider)`, `box-shadow: 0 24px 48px #000000DD`. Backdrop is full-viewport `rgba(0,0,0,0.7)` with `backdrop-filter: blur(6px)`.
- [ ] **3.5** Dialog header: vertical title-group (gap 4) — title "Add Tile" Inter / 24 / 500 / `var(--cui-text-primary)`; subtitle "Choose a tile to add to your dashboard" Inter / 14 / 400 / `var(--cui-text-secondary)`. Close icon-button on the right: 36×36 circular `var(--cui-surface-3)` fill, `close` icon 18×18 `var(--cui-text-primary)`. Padding `24px 24px 16px 24px`, `justify-content: space-between`.
- [ ] **3.6** A 1px `var(--cui-divider)` rule sits below the header and above the action row (full-width).
- [ ] **3.7** Dialog body — tile-grid: `display: grid; grid-template-columns: repeat(4, 1fr); gap: 12px; padding: 20px 24px;`. Auto-flow into multiple rows. Each cell is 120-tall, vertical layout, centred (`justify-content: center; align-items: center`), gap 10, padding 12, `border-radius: var(--cui-radius-md)`, 1px `var(--cui-divider)` border, `var(--cui-surface-2)` fill. Cell contents: a 40×40 circular `var(--cui-primary-dim)` icon container holding a 22×22 Material Symbols Rounded icon, then the tile label (Inter 12 / 500 / `var(--cui-text-primary)`).
- [ ] **3.8** Selected-cell state: fill `var(--cui-accent)`, 2px `var(--cui-accent)` border, drop-shadow `0 4px 12px #FF408155`. Icon container becomes `rgba(255,255,255,0.2)`, icon and label become `var(--cui-on-accent)`, label weight 700. A 18×18 `var(--cui-on-accent)` check badge with 12×12 `check` icon (`var(--cui-accent)` colour) is absolutely positioned at the top-right of the cell (offset right:8 top:6).
- [ ] **3.9** Dialog action row: padding `16px 24px`, gap 8, `justify-content: flex-end`, `align-items: center`. Buttons: `Btn/Basic` "CANCEL" and `Btn/Raised/Primary` "ADD TILE". `ADD TILE` is disabled until a cell is selected. Buttons use Inter / 14 / 500 / `letter-spacing: 0.6`.
- [ ] **3.10** Cancel-click and close-icon-click both close without changes. ADD-TILE-click commits the selected tile via `layoutStore.addTile()` and closes.
- [ ] **3.11** Decide the fate of the legacy `master-page.component.*` and `dashboard-page.component.*` files. They are unrouted and contain a different (Mat) implementation of the same flow. If the new dashboard-shell + dialog supersedes them, delete the legacy files; if they will be used for non-dashboard pages, raise a separate ADR.

See [_reference.md](_reference.md) for tokens and node IDs.
