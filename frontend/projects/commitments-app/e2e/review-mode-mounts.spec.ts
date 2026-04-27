import { expect, test } from '@playwright/test';

import { DashboardPage } from './pages/dashboard.page';

test.describe('dashboard review mode', () => {
  test('switching to review does not throw an Angular DI invalidFactory error', async ({ page }) => {
    const errors: string[] = [];
    page.on('pageerror', (e) => errors.push(`pageerror: ${e.message}`));
    page.on('console', (msg) => {
      if (msg.type() === 'error') {
        errors.push(`console.error: ${msg.text()}`);
      }
    });

    const dashboard = new DashboardPage(page);
    await dashboard.goto();

    await page.locator('.mode-toggle__segment--review').first().click();
    await page.waitForTimeout(800);

    const diErrors = errors.filter((e) => /invalidFactory|DependencyInjection/i.test(e));
    expect(diErrors).toEqual([]);
  });
});
