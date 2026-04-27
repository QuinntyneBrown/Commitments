import { expect, test } from '@playwright/test';

import { DashboardModePage } from './pages/dashboard-mode.page';
import { DashboardPage } from './pages/dashboard.page';

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

  test('Daily Results status pill follows the dashboard mode (Live ↔ Review)', async ({ page }) => {
    const dashboard = new DashboardPage(page);
    const dailyResults = dashboard.tileByTitle('Daily Results');
    const status = dailyResults.locator('.tile-shell__status');

    await expect(status).toHaveText('Live');

    await mode.switchToReview();
    await expect(status).toHaveText('Review');

    await mode.switchToLive();
    await expect(status).toHaveText('Live');
  });
});
