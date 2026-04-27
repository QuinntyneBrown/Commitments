# Bug 007 — ReviewGoalHistoryPage uses getByRole('tab') for mode toggle

**Status**: Fixed

## Description

`review-goal-history.page.ts` locates the Review mode toggle segment with `getByRole('tab', { name: /review/i })`. The mode toggle uses `mat-button-toggle` (rendered as `<button>` elements, not `<tab>` roles), so the locator times out.

## Affected file

`frontend/projects/commitments-app/e2e/pages/review-goal-history.page.ts`

## Fix

Replace `getByRole('tab', { name: /review/i })` with the CSS-class selector used by the fixed `DashboardModePage`: `.mode-toggle__segment--review` within `getByTestId('dashboard-mode-toggle')`.
