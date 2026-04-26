import { test } from '@playwright/test';

import { DashboardModePage } from './pages/dashboard-mode.page';

test.describe('dashboard mode shell', () => {
  let mode: DashboardModePage;

  test.beforeEach(async ({ page }) => {
    mode = new DashboardModePage(page);
    await mode.goto();
  });

  test('renders the mode toggle and defaults to live', async () => {
    await mode.expectLiveActive();
  });

  test('switching to review hides the add-tile FAB and reveals the scrubber slot', async () => {
    await mode.switchToReview();
    await mode.expectReviewActive();
  });

  test('switching back to live restores the FAB and hides the scrubber slot', async () => {
    await mode.switchToReview();
    await mode.expectReviewActive();

    await mode.switchToLive();
    await mode.expectLiveActive();
  });
});
