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

  test('edit-mode-enter button uses the stroked-on-dark fill token (rgba white 13%)', async () => {
    expect(await dashboard.editModeEnterBackground()).toBe('rgba(255, 255, 255, 0.13)');
  });

  test('edit-mode-enter button shows an "EDIT" text label', async () => {
    const label = dashboard.editModeEnterButton.locator('.dashboard-shell__edit-label');
    await expect(label).toBeVisible();
    await expect(label).toHaveText(/edit/i);
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

  test('Outstanding To‑Dos tile description uses the dark-theme secondary text token', async () => {
    expect(await dashboard.outstandingTodosCopyColor()).toBe('rgb(176, 176, 176)');
  });

  test('Daily Results tile metric label uses the dark-theme secondary text token', async () => {
    expect(await dashboard.dailyResultsLabelColor()).toBe('rgb(176, 176, 176)');
  });

  test('Daily Results progress track uses the divider token (no bright bar)', async () => {
    expect(await dashboard.progressTrackBackground()).toBe('rgb(58, 58, 58)');
  });

  test('Outstanding To‑Dos count metric uses the on-palette warning amber', async () => {
    expect(await dashboard.outstandingTodosCountColor()).toBe('rgb(255, 167, 38)');
  });

  test('Daily Results metric uses the on-palette success green', async () => {
    expect(await dashboard.dailyResultsMetricColor()).toBe('rgb(102, 187, 106)');
  });

  test('Daily Results progress fill matches the metric --cui-success', async () => {
    expect(await dashboard.progressFillBackground()).toBe('rgb(102, 187, 106)');
  });

  test('tile-chrome remove button uses --cui-warn (red) in edit mode', async () => {
    await dashboard.enterEditMode();
    expect(await dashboard.removeTileButtonBackground()).toBe('rgb(244, 67, 54)');
    await dashboard.exitEditMode();
  });

  test('tile-chrome shows accent DRAG chip in edit mode', async () => {
    await dashboard.enterEditMode();
    const dragChip = dashboard.page.locator('.tile-chrome__drag').first();
    await expect(dragChip).toBeVisible();
    const dragLabel = dragChip.locator('.tile-chrome__drag-label');
    await expect(dragLabel).toHaveText(/drag/i);
    await dashboard.exitEditMode();
  });

  test('mode-toggle hides Material mat-pseudo-checkbox inside its segments', async () => {
    expect(await dashboard.modeTogglePseudoCheckboxVisibleCount()).toBe(0);
  });

  test('review scrubber background pins to --cui-surface (#1E1E1E)', async () => {
    await dashboard.switchToReview();
    expect(await dashboard.reviewScrubberBackground()).toBe('rgb(30, 30, 30)');
  });

  test('review scrubber jump-to-today button shows visible text label', async ({ page }) => {
    await dashboard.switchToReview();
    const todayBtn = page.getByTestId('review-scrubber-today');
    await expect(todayBtn).toBeVisible();
    const label = todayBtn.locator('.review-scrubber__today-label');
    await expect(label).toBeVisible();
    await expect(label).toHaveText(/today/i);
  });

  test('Outstanding To-Dos copy uses 2-line clamp ellipsis (no mid-word clip)', async () => {
    expect(await dashboard.outstandingTodosCopyLineClamp()).toBe('2');
  });

  test('loads the dashboard with registered plugin tiles', async () => {
    await dashboard.expectDefaultDashboard();
    // Default mode is 'live'; the catalog is filtered to live-supporting tiles.
    // Review Goal History has supportedModes: ['review'] and is hidden here.
    await dashboard.openAddTileDialog();
    await expect(dashboard.addTileDialogLabels).toHaveText([
      'Daily Results',
      'Weekly Focus',
      'Monthly Progress',
      'Outstanding To‑Dos',
      'Relations',
      'Consistency Trend',
      'Live Goal Metrics',
    ]);
    await dashboard.cancelAddTileDialog();
  });

  test('add-tile catalog cells each show a tile-specific icon (not the generic dashboard fallback)', async () => {
    await dashboard.openAddTileDialog();
    const icons = await dashboard.addTileDialog.locator('.add-tile-dialog__icon').allInnerTexts();
    // Every icon must be non-empty and none may be the 'dashboard' fallback
    expect(icons.length).toBeGreaterThan(0);
    for (const icon of icons) {
      expect(icon.trim()).not.toBe('dashboard');
    }
    await dashboard.cancelAddTileDialog();
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

  test('edit mode remove returns correct tile count', async () => {
    await dashboard.enterEditMode();
    await dashboard.removeFirstTile();
    await dashboard.exitEditMode();

    await expect(dashboard.tiles).toHaveCount(4);
    expect(await dashboard.persistedTileCount()).toBe(4);
  });
});
