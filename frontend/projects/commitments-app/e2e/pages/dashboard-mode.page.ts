import { expect, Locator, Page } from '@playwright/test';

import { DashboardPage } from './dashboard.page';

export class DashboardModePage {
  readonly page: Page;
  readonly dashboard: DashboardPage;
  readonly modeToggle: Locator;
  readonly liveSegment: Locator;
  readonly reviewSegment: Locator;
  readonly addTileFab: Locator;
  readonly scrubberSlot: Locator;

  constructor(page: Page) {
    this.page = page;
    this.dashboard = new DashboardPage(page);
    this.modeToggle = page.getByTestId('dashboard-mode-toggle');
    this.liveSegment = this.modeToggle.locator('.mode-toggle__segment--live');
    this.reviewSegment = this.modeToggle.locator('.mode-toggle__segment--review');
    this.addTileFab = page.getByTestId('add-tile-fab');
    this.scrubberSlot = page.getByTestId('dashboard-scrubber-slot');
  }

  async goto() {
    await this.dashboard.goto();
  }

  async switchToReview() {
    await this.reviewSegment.click();
  }

  async switchToLive() {
    await this.liveSegment.click();
  }

  async expectLiveActive() {
    await expect(this.liveSegment).toHaveClass(/mode-toggle__segment--active/);
    await expect(this.addTileFab).toBeVisible();
    await expect(this.scrubberSlot).toBeHidden();
  }

  async expectReviewActive() {
    await expect(this.reviewSegment).toHaveClass(/mode-toggle__segment--active/);
    await expect(this.addTileFab).toBeHidden();
    await expect(this.scrubberSlot).toBeVisible();
  }
}
