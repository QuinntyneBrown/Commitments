import { expect, test } from '@playwright/test';

import { DashboardPage } from './pages/dashboard.page';

test.describe('edit mode', () => {
  let dashboard: DashboardPage;

  test.beforeEach(async ({ page }) => {
    dashboard = new DashboardPage(page);
    await dashboard.goto();
  });

  test('entering edit mode shows EDIT MODE pill and DONE button', async ({ page }) => {
    await page.getByTestId('edit-mode-enter').click();

    await expect(page.getByTestId('edit-mode-pill')).toBeVisible();
    await expect(page.getByTestId('edit-mode-done')).toBeVisible();
  });

  test('entering edit mode hides the mode toggle', async ({ page }) => {
    await page.getByTestId('edit-mode-enter').click();

    await expect(page.getByTestId('dashboard-mode-toggle')).toHaveCount(0);
  });

  test('DONE button exits edit mode and restores mode toggle', async ({ page }) => {
    await page.getByTestId('edit-mode-enter').click();
    await page.getByTestId('edit-mode-done').click();

    await expect(page.getByTestId('dashboard-mode-toggle')).toBeVisible();
    await expect(page.getByTestId('edit-mode-pill')).toHaveCount(0);
    await expect(page.getByTestId('edit-mode-done')).toHaveCount(0);
  });
});
