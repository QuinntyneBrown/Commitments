import { expect, Locator, Page } from '@playwright/test';

const LAYOUT_STORAGE_KEY = 'commitments.layout.live';

export const DEFAULT_TILE_TITLES = [
  'Daily Results',
  'Weekly Focus',
  'Monthly Progress',
  'Outstanding To Dos',
  'Relations',
] as const;

export class DashboardPage {
  readonly page: Page;
  readonly shell: Locator;
  readonly heading: Locator;
  readonly grid: Locator;
  readonly tiles: Locator;
  readonly tileTitles: Locator;
  readonly tileSelect: Locator;
  readonly tileOptions: Locator;
  readonly addTileButton: Locator;
  readonly editLayoutButton: Locator;
  readonly resetLayoutButton: Locator;
  readonly removeTileButtons: Locator;

  constructor(page: Page) {
    this.page = page;
    this.shell = page.getByTestId('dashboard-shell');
    this.heading = page.getByRole('heading', { name: 'Dashboard' });
    this.grid = page.getByTestId('dashboard-grid');
    this.tiles = page.getByTestId('dashboard-tile');
    this.tileTitles = page.getByTestId('tile-title');
    this.tileSelect = page.getByTestId('tile-select');
    this.tileOptions = this.tileSelect.locator('option');
    this.addTileButton = page.getByTestId('add-tile');
    this.editLayoutButton = page.getByTestId('edit-layout');
    this.resetLayoutButton = page.getByTestId('reset-layout');
    this.removeTileButtons = page.getByTestId('remove-tile');
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

    await this.page.goto('/');
    await this.waitUntilReady();
  }

  async waitUntilReady() {
    await expect(this.shell).toBeVisible();
    await expect(this.heading).toBeVisible();
    await expect(this.grid).toBeVisible();
  }

  async expectDefaultDashboard() {
    await expect(this.tiles).toHaveCount(DEFAULT_TILE_TITLES.length);

    for (const title of DEFAULT_TILE_TITLES) {
      await expect(this.tileByTitle(title)).toBeVisible();
    }
  }

  async addTile(displayName: string) {
    await this.tileSelect.selectOption({ label: displayName });
    await this.addTileButton.click();
  }

  async enterEditMode() {
    await this.editLayoutButton.click();
    await expect(this.editLayoutButton).toHaveText('Done');
    await expect(this.removeTileButtons.first()).toBeVisible();
  }

  async exitEditMode() {
    await this.editLayoutButton.click();
    await expect(this.editLayoutButton).toHaveText('Edit Layout');
    await expect(this.removeTileButtons).toHaveCount(0);
  }

  async removeFirstTile() {
    await this.removeTileButtons.first().click();
  }

  async resetLayout() {
    await this.resetLayoutButton.click();
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
    return this.page.locator('.dashboard-shell__topbar').first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  gridBackgroundColor(): Promise<string> {
    return this.page.locator('.dashboard-grid').first()
      .evaluate((el) => getComputedStyle(el).backgroundColor);
  }

  addTileButtonBackground(): Promise<string> {
    return this.addTileButton.evaluate((el) => getComputedStyle(el).backgroundColor);
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
}
