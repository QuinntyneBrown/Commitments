import { expect, test } from '@playwright/test';
import { LoginPage } from './pages/login.page';

test.describe('login page', () => {
  let login: LoginPage;

  test.beforeEach(async ({ page }) => {
    login = new LoginPage(page);
    await login.goto();
  });

  test('renders the login form at /login', async () => {
    await expect(login.heading).toBeVisible();
    await expect(login.usernameInput).toBeVisible();
    await expect(login.passwordInput).toBeVisible();
    await expect(login.submitButton).toBeDisabled();
  });
});
