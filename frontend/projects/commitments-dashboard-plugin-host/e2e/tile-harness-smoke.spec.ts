import { expect, test } from '@playwright/test';
import { TileHarnessPage } from './pages/tile-harness.page';

test.describe('tile harness route', () => {
  let harness: TileHarnessPage;

  test.beforeEach(async ({ page }) => {
    harness = new TileHarnessPage(page);
  });

  test('renders daily-results tile shell', async () => {
    await harness.goto('commitments.daily-results');
    await expect(harness.tile()).toBeVisible();
  });

  test('shows tile-not-found for unknown tileId', async ({ page }) => {
    await page.goto('/tile/does-not-exist');
    await expect(page.getByTestId('tile-not-found')).toBeVisible();
  });
});
