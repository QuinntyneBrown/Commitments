# 05 — Dashboard page layout

**Design**: Frame `OxYKj` (Dashboard — LG 1280, 1280×900). Layout:
- Toolbar (64h) on top.
- Body row: sidenav (250w) + content area (1030 wide). The content area has the PrimaryHeader (80h, "Dashboard") then a 24-padded grid of `Dashboard-Tile` (`0UH2Q`) nodes — 320×200 each, gap 24.
- FAB at bottom-right.

Live variant `39pLD` includes a `Mode-Toggle` aligned right inside the PrimaryHeader.

Edit-mode variant `fJpM0` flips the PrimaryHeader background from `$primary-dim` to `$accent-strong`, replaces the right-side mode toggle with a "DONE" button, and adds an "EDIT MODE" pill next to the title — see [11-edit-mode.md](11-edit-mode.md).

**Implementation**: `DashboardShellComponent` + `DashboardGridComponent`. No sidenav, single combined header row, gridster-based grid, no FAB.

## Tasks
- [ ] **5.1** After [01](01-app-shell-toolbar-primary-header.md)–[04](04-sidenav.md) land, the content area of the shell should already be `flex: 1`, `overflow: auto`, padded 24, with `var(--cui-bg)` background. Confirm `dashboard-grid` fills it without inner padding (gridster manages its own gutter).
- [ ] **5.2** Place the `cui-mode-toggle` on the right side of the PrimaryHeader (live/review variants in design — `39pLD`, `TFRTa`). Hide it on screens that aren't the dashboard.
- [ ] **5.3** Dashboard-grid empty state copy (`dashboard-grid.component.html` lines 3–6): change "Use Add Tile to place a registered plugin tile." to refer to the new FAB ("Tap the **+** button to add a tile.") so the copy still matches the UI after [02](02-remove-extraneous-header-buttons.md).
- [ ] **5.4** Verify gridster gutter and tile spacing match the design's gap **24** (it currently uses gridster's defaults).
- [ ] **5.5** When `editMode === true` (see [11-edit-mode.md](11-edit-mode.md)), the PrimaryHeader of the dashboard page must:
  - Use `var(--cui-accent-strong)` background instead of `var(--cui-primary-dim)`.
  - Render the "EDIT MODE" pill next to the page title.
  - Render the "DONE" button on the right (replaces the mode toggle for the duration of edit mode).

See [_reference.md](_reference.md) for tokens and node IDs.
