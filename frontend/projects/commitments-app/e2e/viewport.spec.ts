import { expect, test } from '@playwright/test';

import { DashboardModePage } from './pages/dashboard-mode.page';

const VIEWPORTS = [
  { name: 'tablet', width: 768, height: 1024 },
  { name: 'lg-desktop', width: 1280, height: 800 },
  { name: 'xl-desktop', width: 1920, height: 1080 }
] as const;

test.describe('responsive dashboard shell', () => {
  for (const viewport of VIEWPORTS) {
    test(`mode toggle and shell render at ${viewport.name} (${viewport.width}x${viewport.height})`, async ({ page }) => {
      await page.setViewportSize({ width: viewport.width, height: viewport.height });

      const mode = new DashboardModePage(page);
      await mode.goto();

      await expect(mode.modeToggle).toBeVisible();
      await mode.expectLiveActive();

      await mode.switchToReview();
      await mode.expectReviewActive();
    });
  }
});
