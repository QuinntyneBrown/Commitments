import { Locator, Page } from '@playwright/test';

export class LoginPage {
  readonly page: Page;
  readonly heading: Locator;
  readonly usernameInput: Locator;
  readonly passwordInput: Locator;
  readonly submitButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.heading = page.getByRole('heading', { name: 'Commitments' });
    this.usernameInput = page.locator('#username');
    this.passwordInput = page.locator('#password');
    this.submitButton = page.getByRole('button', { name: /submit/i });
  }

  async goto() {
    await this.page.goto('/login');
  }

  bodyBackgroundColor(): Promise<string> {
    return this.page.evaluate(() => getComputedStyle(document.body).backgroundColor);
  }

  cardWidth(): Promise<number> {
    return this.page.locator('mat-card').first().evaluate((el) => el.getBoundingClientRect().width);
  }

  cardVerticalCenter(): Promise<number> {
    return this.page.locator('mat-card').first().evaluate((el) => {
      const r = el.getBoundingClientRect();
      return r.top + r.height / 2;
    });
  }

  submitButtonWidth(): Promise<number> {
    return this.submitButton.evaluate((el) => el.getBoundingClientRect().width);
  }

  bodyFontFamily(): Promise<string> {
    return this.page.evaluate(() => getComputedStyle(document.body).fontFamily);
  }

  cardBorderRadius(): Promise<string> {
    return this.page.locator('mat-card').first().evaluate((el) => getComputedStyle(el).borderRadius);
  }
}
