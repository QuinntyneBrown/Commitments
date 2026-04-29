import { expect, Locator, Page } from '@playwright/test';

const LAYOUT_STORAGE_KEY = 'commitments.layout.live';

export const DEFAULT_TILE_TITLES = [
  'Daily Results',
  'Weekly Focus',
  'Monthly Progress',
  'Outstanding Todos',
  'Relations',
] as const;

export class DashboardPage {
  readonly page: Page;
  readonly shell: Locator;
  readonly heading: Locator;
  readonly grid: Locator;
  readonly tiles: Locator;
  readonly tileTitles: Locator;
  readonly addTileButton: Locator;
  readonly addTileDialog: Locator;
  readonly addTileDialogLabels: Locator;
  readonly addTileDialogConfirm: Locator;
  readonly addTileDialogCancel: Locator;
  readonly removeTileButtons: Locator;
  readonly editModeEnterButton: Locator;
  readonly editModeDoneButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.shell = page.getByTestId('dashboard-shell');
    this.heading = page.getByRole('heading', { name: 'Dashboard' });
    this.grid = page.getByTestId('dashboard-grid');
    this.tiles = page.getByTestId('dashboard-tile');
    this.tileTitles = page.getByTestId('tile-title');
    this.addTileButton = page.getByTestId('add-tile-fab');
    this.addTileDialog = page.getByTestId('add-tile-dialog');
    this.addTileDialogLabels = this.addTileDialog.locator('.add-tile-dialog__label');
    this.addTileDialogConfirm = page.getByTestId('add-tile-dialog-confirm');
    this.addTileDialogCancel = page.getByTestId('add-tile-dialog-cancel');
    this.removeTileButtons = page.getByTestId('remove-tile');
    this.editModeEnterButton = page.getByTestId('edit-mode-enter');
    this.editModeDoneButton = page.getByTestId('edit-mode-done');
  }

  async goto(options: { clearStorage?: boolean } = {}) {
    if (options.clearStorage ?? true) {
      await this.page.addInitScript((storageKey) => {
        const setupKey = `${storageKey}.e2e-cleared`;
        if (!sessionStorage.getItem(setupKey)) {
          localStorage.removeItem(storageKey);
          sessionStorage.setItem(setupKey, 'true');
        }
      }, LAYOUT_STORAGE_KEY);
    }

    // Use the real login flow so the Angular app handles auth state correctly
    await this.page.goto('/login');
    await this.page.fill('[id="username"]', 'quinntynebrown@gmail.com');
    await this.page.fill('[id="password"]', 'P@ssw0rd');
    await this.page.click('button[type="submit"]');

    await this.page.waitForURL('/');
    await this.waitUntilReady();
  }

  async waitUntilReady() {
    await expect(this.shell).toBeVisible({ timeout: 10000 });
    await expect(this.heading).toBeVisible({ timeout: 10000 });
    await expect(this.grid).toBeVisible({ timeout: 15000 });
  }

  async expectDefaultDashboard() {
    await expect(this.tiles).toHaveCount(DEFAULT_TILE_TITLES.length);

    for (const title of DEFAULT_TILE_TITLES) {
      await expect(this.tileByTitle(title)).toBeVisible();
    }
  }

  async openAddTileDialog() {
    await this.addTileButton.click();
    await expect(this.addTileDialog).toBeVisible();
  }

  async cancelAddTileDialog() {
    await this.addTileDialogCancel.click();
    await expect(this.addTileDialog).toHaveCount(0);
  }

  async addTile(displayName: string) {
    await this.openAddTileDialog();
    await this.addTileDialog.locator('.add-tile-dialog__cell', { hasText: displayName }).click();
    await this.addTileDialogConfirm.click();
    await expect(this.addTileDialog).toHaveCount(0);
  }

  async enterEditMode() {
    await this.editModeEnterButton.click();
    await expect(this.editModeDoneButton).toBeVisible();
    await expect(this.removeTileButtons.first()).toBeVisible();
  }

  async exitEditMode() {
    await this.editModeDoneButton.click();
    await expect(this.editModeEnterButton).toBeVisible();
    await expect(this.removeTileButtons).toHaveCount(0);
  }

  async removeFirstTile() {
    await this.removeTileButtons.first().click();
  }

  async reloadWithoutClearingStorage() {
    await this.page.reload();
    await this.waitUntilReady();
  }

  async persistedTileCount(): Promise<number> {
    return await this.page.evaluate((storageKey) => {
      const raw = localStorage.getItem(storageKey);
      return raw ? JSON.parse(raw).items.length : 0;
    }, LAYOUT_STORAGE_KEY);
  }

  tileByTitle(title: string): Locator {
    return this.page.getByTestId('tile-shell').filter({ has: this.page.getByTestId('tile-title').filter({ hasText: title }) });
  }

  topbarBackgroundColor(): Promise<string> {
    return this.page.locator('.dashboard-layout__toolbar').first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  gridBackgroundColor(): Promise<string> {
    return this.page.locator('.dashboard-grid').first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  addTileButtonBackground(): Promise<string> {
    return this.addTileButton.evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  editModeEnterBackground(): Promise<string> {
    return this.editModeEnterButton.evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  inactiveModeSegmentBackground(): Promise<string> {
    return this.page
      .locator('.mode-toggle__segment:not(.mode-toggle__segment--active)')
      .first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  weeklyFocusLabelColor(): Promise<string> {
    return this.page
      .locator('.focus-list strong')
      .first()
      .evaluate((el) => getComputedStyle(el).color);
  }

  relationsValueColor(): Promise<string> {
    return this.page
      .locator('.relations strong')
      .first()
      .evaluate((el) => getComputedStyle(el).color);
  }

  outstandingTodosCopyColor(): Promise<string> {
    return this.page
      .locator('.todo-copy')
      .first()
      .evaluate((el) => getComputedStyle(el).color);
  }

  dailyResultsLabelColor(): Promise<string> {
    return this.page
      .locator('.metric__label')
      .first()
      .evaluate((el) => getComputedStyle(el).color);
  }

  progressTrackBackground(): Promise<string> {
    return this.page
      .locator('.progress')
      .first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  outstandingTodosCountColor(): Promise<string> {
    return this.page
      .locator('.todo-count')
      .first()
      .evaluate((el) => getComputedStyle(el).color);
  }

  dailyResultsMetricColor(): Promise<string> {
    return this.page
      .locator('.metric__value')
      .first()
      .evaluate((el) => getComputedStyle(el).color);
  }

  progressFillBackground(): Promise<string> {
    return this.page
      .locator('.progress span')
      .first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  removeTileButtonBackground(): Promise<string> {
    return this.removeTileButtons.first().evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  modeTogglePseudoCheckboxVisibleCount(): Promise<number> {
    return this.page.locator('.mode-toggle .mat-pseudo-checkbox:visible').count();
  }

  async switchToReview(): Promise<void> {
    await this.page.locator('.mode-toggle__segment--review').first().click();
    await this.page.waitForTimeout(400);
  }

  reviewScrubberBackground(): Promise<string> {
    return this.page.locator('.review-scrubber').first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  outstandingTodosCopyLineClamp(): Promise<string> {
    return this.page.locator('.todo-copy').first()
      .evaluate((el) => getComputedStyle(el).webkitLineClamp);
  }

  topbarOverflowPx(): Promise<number> {
    return this.page.locator('.dashboard-layout__toolbar').first()
      .evaluate((el) => Math.max(0, el.scrollWidth - el.clientWidth));
  }

  tileHeaderIconColor(): Promise<string> {
    return this.page.locator('.tile-shell__icon').first()
      .evaluate((el) => getComputedStyle(el).color);
  }
}
