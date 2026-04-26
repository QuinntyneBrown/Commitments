import { expect, Locator, Page } from '@playwright/test';

import { DashboardPage } from './dashboard.page';

export class ReviewGoalHistoryPage {
  readonly page: Page;
  readonly dashboard: DashboardPage;
  readonly tile: Locator;
  readonly title: Locator;
  readonly count: Locator;
  readonly target: Locator;
  readonly reviewBadge: Locator;
  readonly dateBadge: Locator;
  readonly reviewToggle: Locator;

  constructor(page: Page) {
    this.page = page;
    this.dashboard = new DashboardPage(page);
    this.tile = page.getByTestId('review-goal-history-tile');
    this.title = this.tile.getByTestId('tile-title');
    this.count = this.tile.getByTestId('review-goal-history-count');
    this.target = this.tile.getByTestId('review-goal-history-target');
    this.reviewBadge = this.tile.getByTestId('review-goal-history-review-badge');
    this.dateBadge = this.tile.getByTestId('review-goal-history-date-badge');
    this.reviewToggle = page.getByTestId('dashboard-mode-toggle').getByRole('tab', { name: /review/i });
  }

  async goto() {
    await this.dashboard.goto();
  }

  async addReviewGoalHistoryTile() {
    // Tile is review-only after design 10; switch modes before opening the catalog.
    await this.reviewToggle.click();
    await this.dashboard.addTile('Review Goal History');
  }

  async expectVisible() {
    await expect(this.tile).toBeVisible();
    await expect(this.title).toHaveText('Review Goal History');
    await expect(this.reviewBadge).toBeVisible();
  }
}
