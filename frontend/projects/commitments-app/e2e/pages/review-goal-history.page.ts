import { expect, Locator, Page } from '@playwright/test';

import { DashboardPage } from './dashboard.page';

const REVIEW_LAYOUT_KEY = 'commitments.layout.review';
const MODE_KEY = 'commitments.dashboardMode';

const REVIEW_TILE_SEED = JSON.stringify({
  schemaVersion: 1,
  savedAt: 0,
  items: [{
    instanceId: 'e2e-review-goal-history',
    tileId: 'commitments.review-goal-history',
    cols: 3,
    rows: 3,
    x: 0,
    y: 0
  }]
});

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
    this.reviewToggle = page.getByTestId('dashboard-mode-toggle').locator('.mode-toggle__segment--review');
  }

  async goto() {
    await this.dashboard.goto();
  }

  async addReviewGoalHistoryTile() {
    // The Add Tile FAB is live-only by design; seed the review layout directly
    // and switch to review mode so the tile renders.
    await this.page.addInitScript((args: { layoutKey: string; modeKey: string; seed: string }) => {
      const setupKey = `${args.layoutKey}.e2e-seeded`;
      if (!sessionStorage.getItem(setupKey)) {
        localStorage.setItem(args.layoutKey, args.seed);
        localStorage.setItem(args.modeKey, 'review');
        sessionStorage.setItem(setupKey, 'true');
      }
    }, { layoutKey: REVIEW_LAYOUT_KEY, modeKey: MODE_KEY, seed: REVIEW_TILE_SEED });
    await this.page.goto('/');
    await this.dashboard.waitUntilReady();
  }

  async expectVisible() {
    await expect(this.tile).toBeVisible();
    await expect(this.title).toHaveText('Review Goal History');
    await expect(this.reviewBadge).toBeVisible();
  }
}
