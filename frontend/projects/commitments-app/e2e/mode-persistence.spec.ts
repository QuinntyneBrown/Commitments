import { test } from '@playwright/test';

import { DashboardModePage } from './pages/dashboard-mode.page';

test.describe('dashboard mode persistence', () => {
  test('persists the selected mode across reloads', async ({ page }) => {
    const mode = new DashboardModePage(page);
    await mode.goto();
    await mode.expectLiveActive();

    await mode.switchToReview();
    await mode.expectReviewActive();

    await page.reload();
    await mode.expectReviewActive();
  });
});
