import { expect, Locator, Page } from '@playwright/test';

import { DashboardPage } from './dashboard.page';

export class LiveGoalMetricsPage {
  readonly page: Page;
  readonly dashboard: DashboardPage;
  readonly tile: Locator;
  readonly title: Locator;
  readonly count: Locator;
  readonly target: Locator;
  readonly liveBadge: Locator;

  constructor(page: Page) {
    this.page = page;
    this.dashboard = new DashboardPage(page);
    this.tile = page.getByTestId('live-goal-metrics-tile');
    this.title = this.tile.getByTestId('tile-title');
    this.count = this.tile.getByTestId('live-goal-metrics-count');
    this.target = this.tile.getByTestId('live-goal-metrics-target');
    this.liveBadge = this.tile.getByTestId('live-goal-metrics-live-badge');
  }

  async goto() {
    await this.dashboard.goto();
  }

  async addLiveGoalMetricsTile() {
    await this.dashboard.addTile('Live Goal Metrics');
  }

  async expectVisible() {
    await expect(this.tile).toBeVisible();
    await expect(this.title).toHaveText('Live Goal Metrics');
    await expect(this.liveBadge).toBeVisible();
  }
}
