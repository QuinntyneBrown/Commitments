import { expect, test } from '@playwright/test';
import { TileHarnessPage } from './pages/tile-harness.page';
import { getBridge } from './support/bridge';

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

  test('bridge reflects tileId, mode and asOf after navigation', async ({ page }) => {
    harness = new TileHarnessPage(page);
    await harness.goto('commitments.daily-results', { mode: 'review', asOf: '2026-04-01' });
    const snapshot = await getBridge(page);
    expect(snapshot.tileId).toBe('commitments.daily-results');
    expect(snapshot.mode).toBe('review');
    expect(snapshot.asOf).toBe('2026-04-01');
    expect(snapshot.http).toEqual([]);
    expect(snapshot.chart).toEqual([]);
  });
});
