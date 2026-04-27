# 06 — Tile chrome (default + editable) and tile card styling

**Status: ACCEPTED**

> **2026-04-27 update**: the design now defines a dedicated **`Dashboard-Tile/Editable`** component (`KB9Mx`) for use in edit mode, with explicit drag, close, and resize affordances. The default `Dashboard-Tile` (`0UH2Q`) is unchanged.

## Default tile

**Design**: `Dashboard-Tile` (`0UH2Q`) — 320×200, `$surface-2` (#242424), `$r-md` (8) corner, 1px `$divider` stroke, padding 16, gap 8. Header row: 20×20 icon (Material Symbols Rounded, `$primary`) + label (Inter, `$fs-md`/16, `$fw-medium`, `$text-primary`). Body: vertical-centred display number Inter / `$fs-display`/48 / `$fw-bold` / `$accent`, plus secondary text Inter / `$fs-sm`/12 / normal / `$text-secondary`.

**Implementation**: `dashboard-grid.component.scss` — gridster-item padding 16, 1px `$divider`, 8px radius, `$surface-2` background, `$cui-shadow-raised` shadow. Tile chrome uses an `x` text glyph for remove. Tile content lives in plugin tile components (e.g. `daily-results-tile.component.*`) which were not audited in this pass.

## Editable tile (edit mode)

**Design**: `Dashboard-Tile/Editable` (`KB9Mx`) — same 320×200 outer dimensions, `$surface-2` fill, `$r-md` corner, but stroked **2px** with `$accent` (outside align). Vertical layout with three rows:
- **Top chrome row** (height 28, padding `[4, 8]`, justify space-between, align centre):
  - **Drag handle** (left): 36×20 frame, `$accent` fill, `$r-sm` corner, gap 2, contents centred — `drag_indicator` icon 16×16 `$on-accent` + "DRAG" text Inter / 9 / `$fw-bold` / `$on-accent` / letter-spacing 0.5.
  - **Close button** (right): 22×22 circular, `$warn` (#F44336) fill, `$r-full`, centred `close` icon 14×14 `$on-accent`.
- **Body row** (vertical, gap 8, justify centre, padding `[0, 16]`, fill height): same content as default tile (header line, primary number, secondary text).
- **Bottom chrome row** (height 28, padding `[4, 6]`, justify end, align centre):
  - **Resize handle**: 22×22, `$accent` fill, `$r-sm` corner, centred `open_in_full` icon 14×14 `$on-accent`.

**Implementation**: nothing yet. The current edit-affordance is the `x` text glyph in `dashboard-grid.component.html` only.

## Tasks
- [ ] **6.1** Replace the `x` text glyph in `tile-chrome__button` (line 22 of `dashboard-grid.component.html`) with a Material Symbols Rounded `close` icon (20×20). Font matches the rest of the UI. (Default-tile chrome.)
- [ ] **6.2** Audit each plugin tile (`commitments-dashboard-plugin/src/lib/tiles/*-tile/*.component.html|scss`) against `Dashboard-Tile` (`0UH2Q`) and the dashboard-tile examples visible in `OxYKj`/`39pLD`. For each tile, verify: header icon family/name/size/colour, header label typography, primary number typography (Inter / 48 / 700 / accent), secondary label (Inter / 12 / `$text-secondary`). File a sub-task per tile after the audit.
- [ ] **6.3** Tile shell `box-shadow` currently uses `var(--cui-shadow-raised)` — verify against design tile (no explicit shadow on `Dashboard-Tile`; only stroke). Consider removing `box-shadow` on tiles since the design uses border only.
- [ ] **6.4** Add an "editable" tile-chrome variant gated by edit-mode (see [11-edit-mode.md](11-edit-mode.md)). When `editMode === true` for the dashboard:
  - Apply a 2px `var(--cui-accent)` outside border to each tile (replace, do not add to, the default 1px divider border).
  - Render a top-chrome strip (height 28, padding `4px 8px`, `justify-content: space-between`) holding a drag handle (36×20, `var(--cui-accent)` fill, `border-radius: var(--cui-radius-sm)`, `drag_indicator` icon + "DRAG" 9/700/0.5) on the left and a close button (22×22 circular `var(--cui-warn)`, `close` icon) on the right.
  - Render a bottom-chrome strip (height 28, padding `4px 6px`, `justify-content: flex-end`) holding a resize handle (22×22 `var(--cui-accent)` `border-radius: var(--cui-radius-sm)`, `open_in_full` icon).
  - Wire the drag handle to gridster's drag-start handle (so drag is intentional, not anywhere on the tile body), the close button to `layoutStore.removeTile(tileId)`, and the resize handle to gridster's resize-start handle.
- [ ] **6.5** Outside of edit mode, the tile renders with the default chrome (no strips, no close, no drag handle, no resize handle, 1px `var(--cui-divider)` border). Gridster drag/resize handles must be hidden/disabled in non-edit mode.

See [_reference.md](_reference.md) for tokens and node IDs.
