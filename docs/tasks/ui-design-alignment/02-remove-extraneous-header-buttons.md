# 02 — Header has buttons that are not in the design: remove inline tile picker, Edit Layout, Reset

**Status: ACCEPTED**

**Design**: The toolbar/primary-header rows do **not** contain a `<select>` dropdown, an "Add Tile" text button, an "Edit Layout" toggle button, or a "Reset" button. The design uses:
- A floating action button (FAB) anchored bottom-right (`FAB/Accent` `e9DYo`, see [03-add-tile-fab-and-dialog.md](03-add-tile-fab-and-dialog.md)).
- An optional `Mode-Toggle` (`A1yim` / `WFYHQ`) inside the PrimaryHeader on dashboard live/review variants only (see frame `39pLD` "Dashboard Live — LG").

**Implementation** (`dashboard-shell.component.html` lines 16–41): inline `<select>` with tile dropdown, "Add Tile", "Edit Layout"/"Done", "Reset" buttons. None of these exist in the design.

## Tasks
- [ ] **2.1** Remove the inline tile-select `<select>` (lines 17–26) from `dashboard-shell.component.html`.
- [ ] **2.2** Remove the "Add Tile" inline button (line 27). Replace with the FAB flow in [03-add-tile-fab-and-dialog.md](03-add-tile-fab-and-dialog.md).
- [ ] **2.3** Remove the "Edit Layout"/"Done" toggle button (lines 29–37) from the **toolbar**. Edit-mode is a real design state (frame `fJpM0`, see [11-edit-mode.md](11-edit-mode.md)), but the entry/exit affordances live in the **PrimaryHeader** (an `edit` icon-button on the right while live; a "DONE" button while in edit mode), not in the toolbar. Move/replace, do not retain in the toolbar.
- [ ] **2.4** Remove the "Reset" button (lines 38–40).
- [ ] **2.5** Remove `selectedTileId`, `tiles`, `addSelectedTile`, the `effect()` block, and all related styles (`dashboard-shell__select`, `dashboard-shell__button*`) from `dashboard-shell.component.ts/scss`. Strip the `signal`/`computed` imports if no longer used.
- [ ] **2.6** Keep `cui-mode-toggle` but place it inside the PrimaryHeader row, right-aligned (matches `Dashboard Live — LG` frame `39pLD` and `Dashboard Review — LG` `TFRTa`), not in the toolbar row. Hide it while edit mode is active (see [11.6](11-edit-mode.md)).

See [_reference.md](_reference.md) for tokens and node IDs.
