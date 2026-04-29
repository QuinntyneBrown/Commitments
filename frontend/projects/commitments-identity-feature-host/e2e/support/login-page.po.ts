import { Page } from '@playwright/test';
import { BasePage } from './pom-base';

export class LoginPagePo extends BasePage {
  readonly url = '/login';
  readonly username = this.page.getByLabel('Username');
  readonly password = this.page.getByLabel('Password');
  readonly submit = this.page.getByRole('button', { name: 'Sign in' });

  constructor(page: Page) { super(page); }

  async signIn(username: string, password: string): Promise<void> {
    await this.username.fill(username);
    await this.password.fill(password);
    await this.submit.click();
  }
}
