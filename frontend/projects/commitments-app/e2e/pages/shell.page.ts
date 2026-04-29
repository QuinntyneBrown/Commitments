import { Locator, Page } from '@playwright/test';

export class ShellPage {
  readonly page: Page;
  readonly sidenav: Locator;

  constructor(page: Page) {
    this.page = page;
    this.sidenav = page.getByTestId('dashboard-sidenav');
  }

  async goto() {
    const tokenRes = await this.page.request.post('https://localhost:63713/api/v1.0/users/token', {
      data: { username: 'quinntynebrown@gmail.com', password: 'P@ssw0rd' },
      ignoreHTTPSErrors: true,
    });
    const { accessToken } = await tokenRes.json() as { accessToken: string };
    await this.page.addInitScript((token: string) => {
      localStorage.setItem('accessTokenKey', token);
    }, accessToken);

    await this.page.goto('/');
    await this.page.waitForSelector('[data-testid="dashboard-sidenav"]');
  }

  navItemIcon(label: string): Locator {
    return this.sidenav
      .locator(`[data-testid="sidenav-item-${label}"]`)
      .locator('.sidenav-item__icon');
  }
}
