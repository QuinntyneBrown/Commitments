import { expect, Locator, Page } from '@playwright/test';

import { DashboardPage } from './dashboard.page';

export class DashboardModePage {
  readonly page: Page;
  readonly dashboard: DashboardPage;
  readonly modeToggle: Locator;
  readonly liveSegment: Locator;
  readonly reviewSegment: Locator;
  readonly addTileButton: Locator;
  readonly scrubberSlot: Locator;

  constructor(page: Page) {
    this.page = page;
    this.dashboard = new DashboardPage(page);
    this.modeToggle = page.getByTestId('dashboard-mode-toggle');
    this.liveSegment = this.modeToggle.getByRole('tab', { name: /live/i });
    this.reviewSegment = this.modeToggle.getByRole('tab', { name: /review/i });
    this.addTileButton = page.getByTestId('add-tile');
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
    await expect(this.liveSegment).toHaveAttribute('aria-pressed', 'true');
    await expect(this.addTileButton).toBeVisible();
    await expect(this.scrubberSlot).toBeHidden();
  }

  async expectReviewActive() {
    await expect(this.reviewSegment).toHaveAttribute('aria-pressed', 'true');
    await expect(this.addTileButton).toBeHidden();
    await expect(this.scrubberSlot).toBeVisible();
  }
}
