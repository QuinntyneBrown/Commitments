import { expect, test } from '@playwright/test';

import { DashboardModePage } from './pages/dashboard-mode.page';
import { DashboardPage } from './pages/dashboard.page';

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

  test('topbar does not overflow horizontally at tablet (768x1024)', async ({ page }) => {
    await page.setViewportSize({ width: 768, height: 1024 });
    const dashboard = new DashboardPage(page);
    await dashboard.goto();

    expect(await dashboard.topbarOverflowPx()).toBe(0);
  });

  test('default tiles render at least 120px tall on lg-desktop', async ({ page }) => {
    await page.setViewportSize({ width: 1280, height: 800 });
    const dashboard = new DashboardPage(page);
    await dashboard.goto();

    const firstTileHeight = await dashboard.tiles.first().evaluate(
      (el) => el.getBoundingClientRect().height
    );
    expect(firstTileHeight).toBeGreaterThanOrEqual(120);
  });
});
