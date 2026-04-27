# Bug 001 — `add-tile` testid mismatch

**Status**: Open

## Description

The dashboard shell FAB button has `data-testid="add-tile-fab"` (dashboard-shell.component.html:57), but the Playwright page objects `DashboardModePage` and `DashboardPage` reference `data-testid="add-tile"`. This causes `expectLiveActive()` to always fail (element never found) and makes `expectReviewActive()` pass for the wrong reason (element absent from DOM for both modes).

## Affected files

- `frontend/projects/commitments-app/e2e/pages/dashboard-mode.page.ts` line 20
- `frontend/projects/commitments-app/e2e/pages/dashboard.page.ts` line 36

## Root cause

The FAB testid was renamed to `add-tile-fab` when the FAB was refactored but the page objects were not updated.

## Fix

Change both page objects to use `page.getByTestId('add-tile-fab')`.
