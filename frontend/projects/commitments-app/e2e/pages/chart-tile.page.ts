import { expect, Locator, Page } from '@playwright/test';

import { DashboardPage } from './dashboard.page';

export class ChartTilePage {
  readonly page: Page;
  readonly dashboard: DashboardPage;
  readonly tile: Locator;
  readonly canvas: Locator;

  constructor(page: Page) {
    this.page = page;
    this.dashboard = new DashboardPage(page);
    this.tile = page.getByTestId('tile-shell').filter({ hasText: 'Consistency Trend' });
    this.canvas = page.getByTestId('consistency-trend-canvas');
  }

  async goto() {
    await this.dashboard.goto();
  }

  async addChartTile() {
    await this.dashboard.addTile('Consistency Trend');
  }

  async expectVisible() {
    await expect(this.tile).toBeVisible();
    await expect(this.canvas).toBeVisible();
  }

  async expectCanvasHasDimensions() {
    const box = await this.canvas.boundingBox();
    expect(box?.width ?? 0).toBeGreaterThan(0);
    expect(box?.height ?? 0).toBeGreaterThan(0);
  }
}
