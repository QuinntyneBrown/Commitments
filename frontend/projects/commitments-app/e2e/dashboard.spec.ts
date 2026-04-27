import { expect, test } from '@playwright/test';
import { DashboardPage } from './pages/dashboard.page';

test.describe('dashboard shell', () => {
  let dashboard: DashboardPage;

  test.beforeEach(async ({ page }) => {
    dashboard = new DashboardPage(page);
    await dashboard.goto();
  });

  test('topbar uses the dark toolbar token from the design system', async () => {
    expect(await dashboard.topbarBackgroundColor()).toBe('rgb(31, 34, 51)');
  });

  test('grid area uses the dark app background from the design tokens', async () => {
    expect(await dashboard.gridBackgroundColor()).toBe('rgb(18, 18, 18)');
  });

  test('topbar action buttons use the stroked-on-dark style (transparent fill)', async () => {
    expect(await dashboard.addTileButtonBackground()).toBe('rgba(0, 0, 0, 0)');
  });

  test('inactive mode-toggle segment has no fill (matches the design pill)', async () => {
    expect(await dashboard.inactiveModeSegmentBackground()).toBe('rgba(0, 0, 0, 0)');
  });

  test('Weekly Focus row label uses the dark-theme primary text token', async () => {
    expect(await dashboard.weeklyFocusLabelColor()).toBe('rgb(255, 255, 255)');
  });

  test('Relations tile percentage uses the dark-theme primary text token', async () => {
    expect(await dashboard.relationsValueColor()).toBe('rgb(255, 255, 255)');
  });

  test('loads the dashboard with registered plugin tiles', async () => {
    await dashboard.expectDefaultDashboard();
    // Default mode is 'live'; the dropdown is filtered to live-supporting tiles.
    // Review Goal History has supportedModes: ['review'] and is hidden here.
    await expect(dashboard.tileOptions).toHaveText([
      'Daily Results',
      'Weekly Focus',
      'Monthly Progress',
      'Outstanding To Dos',
      'Relations',
      'Consistency Trend',
      'Live Goal Metrics',
    ]);
  });

  test('toggles edit layout mode and exposes tile chrome', async () => {
    await dashboard.enterEditMode();
    await expect(dashboard.removeTileButtons).toHaveCount(5);

    await dashboard.exitEditMode();
  });

  test('adds a plugin tile and persists the dashboard layout', async () => {
    await dashboard.addTile('Monthly Progress');

    await expect(dashboard.tiles).toHaveCount(6);
    await expect(dashboard.tileByTitle('Monthly Progress')).toHaveCount(2);
    expect(await dashboard.persistedTileCount()).toBe(6);

    await dashboard.reloadWithoutClearingStorage();
    await expect(dashboard.tiles).toHaveCount(6);
    await expect(dashboard.tileByTitle('Monthly Progress')).toHaveCount(2);
  });

  test('removes a tile in edit mode', async () => {
    await dashboard.enterEditMode();
    await dashboard.removeFirstTile();

    await expect(dashboard.tiles).toHaveCount(4);
    expect(await dashboard.persistedTileCount()).toBe(4);
  });

  test('resets the dashboard to the default plugin layout', async () => {
    await dashboard.enterEditMode();
    await dashboard.removeFirstTile();
    await expect(dashboard.tiles).toHaveCount(4);

    await dashboard.resetLayout();

    await dashboard.expectDefaultDashboard();
  });
});
