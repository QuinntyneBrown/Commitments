# 10 — Tests to update / add

## Tasks
- [ ] **10.1** Update `dashboard-shell.component.spec.ts` to remove tile-select / Add Tile button / Edit Layout / Reset assertions, and add: hamburger toggles sidenav, FAB opens dialog, mode-toggle present in primary header (live + review only).
- [ ] **10.2** Add a Playwright e2e test for the FAB → Add-Tile-Dialog → tile-grid-select → ADD TILE flow that mirrors [03](03-add-tile-fab-and-dialog.md). Use Page Object Model under `frontend/projects/commitments-app-e2e/`. Cover: opening, selecting one tile, ADD TILE adds it to the layout, CANCEL discards, close-icon discards, backdrop click does NOT discard (modal). Assert ADD TILE is disabled until a tile is selected.
- [ ] **10.3** Add a Playwright e2e test for the sidenav: each link routes correctly, active state applied, hover overlay applies on mouse-over.
- [ ] **10.4** Visual-regression coverage: add Playwright snapshots at viewport widths 360 / 768 / 1280 / 1920 of the dashboard, login, my-profile, settings screens, and compare to design frames at the same breakpoints. Include `fJpM0` (edit mode) and `a2Cjz` (edit mode + add-tile dialog) at LG (1280).
- [ ] **10.5** Add a Playwright e2e test for **edit mode** entry/exit ([11-edit-mode.md](11-edit-mode.md)): clicking the edit affordance turns the PrimaryHeader pink, shows the "EDIT MODE" pill, hides the mode toggle, shows the "DONE" button, and switches all tiles to the editable chrome (drag handle, close button, resize handle visible). Clicking DONE reverts every one of those changes.
- [ ] **10.6** Add unit tests for the editable tile-chrome ([06.4](06-tile-chrome-and-styling.md)): close button calls `layoutStore.removeTile(id)`, drag handle is wired to gridster's drag-start, resize handle to gridster's resize-start. Drag/resize handles are absent in non-edit mode.

See [_reference.md](_reference.md) for tokens and node IDs.
