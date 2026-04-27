# Bug 003 — DashboardPage page object uses outdated testids

**Status**: Fixed

## Description

`dashboard.page.ts` still references pre-redesign testids that no longer exist in the DOM:
- `edit-layout` → replaced by `edit-mode-enter` (enter) + `edit-mode-done` (exit)
- `reset-layout` → removed; no reset button exists in the new UI
- `tile-select` / `option` → replaced by the FAB (`add-tile-fab`) + `add-tile-dialog` + `add-tile-cell-<id>`

This causes 6 tests in `dashboard.spec.ts` to fail with element-not-found timeouts.

## Affected files

- `frontend/projects/commitments-app/e2e/pages/dashboard.page.ts`
- `frontend/projects/commitments-app/e2e/dashboard.spec.ts`

## Failing tests

- "loads the dashboard with registered plugin tiles"
- "tile-chrome remove button uses --cui-surface-3 in edit mode"
- "toggles edit layout mode and exposes tile chrome"
- "adds a plugin tile and persists the dashboard layout"
- "removes a tile in edit mode"
- "resets the dashboard to the default plugin layout"

## Fix

Update page object methods to use new testids; rewrite `addTile()` to use FAB + dialog; remove `resetLayout()` test (no UI affordance).
