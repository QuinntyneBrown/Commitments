import { expect, test } from '@playwright/test';
import { LoginPage } from './pages/login.page';

const DESIGN_BG = 'rgb(18, 18, 18)';

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

  test('renders on the dark app background from the design tokens', async () => {
    expect(await login.bodyBackgroundColor()).toBe(DESIGN_BG);
  });
});
