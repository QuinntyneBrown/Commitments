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

  test('card width matches the LG design (480px)', async () => {
    expect(await login.cardWidth()).toBe(480);
  });

  test('card is vertically centered in the viewport', async ({ page }) => {
    const viewport = page.viewportSize()!;
    const center = await login.cardVerticalCenter();
    expect(Math.abs(center - viewport.height / 2)).toBeLessThan(20);
  });

  test('Submit button spans the card content width', async () => {
    expect(await login.submitButtonWidth()).toBeGreaterThanOrEqual(380);
  });

  test('typography uses Inter (the design system font)', async () => {
    const family = await login.bodyFontFamily();
    expect(family.toLowerCase()).toContain('inter');
  });

  test('card uses the medium radius token (8px) from the design', async () => {
    expect(await login.cardBorderRadius()).toBe('8px');
  });
});
