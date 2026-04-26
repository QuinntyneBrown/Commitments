import { Page, Locator } from '@playwright/test';

/**
 * Page Object Model for the Home/Dashboard page
 */
export class HomePage {
  readonly page: Page;
  readonly heading: Locator;
  readonly navigationMenu: Locator;

  constructor(page: Page) {
    this.page = page;
    this.heading = page.locator('h1, h2').first();
    this.navigationMenu = page.locator('nav');
  }

  async goto() {
    await this.page.goto('/');
    await this.page.waitForLoadState('networkidle');
  }

  async waitForPageLoad() {
    await this.page.waitForLoadState('domcontentloaded');
  }

  async getTitle(): Promise<string> {
    return await this.page.title();
  }
}
