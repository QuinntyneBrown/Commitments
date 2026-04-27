import { Locator, Page } from '@playwright/test';

export class ShellPage {
  readonly page: Page;
  readonly sidenav: Locator;

  constructor(page: Page) {
    this.page = page;
    this.sidenav = page.getByTestId('dashboard-sidenav');
  }

  async goto() {
    await this.page.goto('/');
    await this.page.waitForSelector('[data-testid="dashboard-sidenav"]');
  }

  navItemIcon(label: string): Locator {
    return this.sidenav
      .locator(`[data-testid="sidenav-item-${label}"]`)
      .locator('.sidenav-item__icon');
  }
}
