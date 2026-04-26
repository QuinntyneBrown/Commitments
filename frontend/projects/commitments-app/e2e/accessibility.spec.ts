import { test, expect } from '@playwright/test';
import AxeBuilder from '@axe-core/playwright';
import { DashboardPage } from './pages/dashboard.page';

test.describe('dashboard accessibility', () => {
  test('has no WCAG A/AA violations on initial dashboard load', async ({ page }) => {
    const dashboard = new DashboardPage(page);
    await dashboard.goto();

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa'])
      .analyze();

    expect(accessibilityScanResults.violations).toEqual([]);
  });
});
