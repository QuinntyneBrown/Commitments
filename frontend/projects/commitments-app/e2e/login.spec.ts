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

  test('card uses the surface token (#1E1E1E) from the design', async () => {
    expect(await login.cardBackgroundColor()).toBe('rgb(30, 30, 30)');
  });

  test('"Login" mat-card-title uses the secondary text token (#B0B0B0)', async () => {
    expect(await login.cardTitleColor()).toBe('rgb(176, 176, 176)');
  });

  test('card uses the dedicated --cui-shadow-login-card token from the design', async () => {
    expect(await login.cardBoxShadow()).toContain('rgba(0, 0, 0, 0.7) 0px 8px 24px');
  });

  test('failed login on 401 shows the "Login Failed" snackbar (design 01-Login Slice C, L2-001 AC #3)', async ({ page }) => {
    await login.usernameInput.fill('bad@user.com');
    await login.passwordInput.fill('wrong');
    await login.submitButton.click();
    const snackbar = page.locator('mat-snack-bar-container');
    await expect(snackbar).toBeVisible();
    const text = await snackbar.innerText();
    expect(text).toContain('Login Failed');
    expect(text).not.toContain('ocurr.');
  });

  test('failed login keeps the form editable and stays on /login (design 01-Login Slice C)', async ({ page }) => {
    await login.usernameInput.fill('bad@user.com');
    await login.passwordInput.fill('wrong');
    await login.submitButton.click();
    await expect(page.locator('mat-snack-bar-container')).toBeVisible();
    expect(page.url()).toContain('/login');
    await expect(login.usernameInput).toBeEnabled();
    await expect(login.passwordInput).toBeEnabled();
  });

  test('error snackbar uses the warn (red) background from the design token', async ({ page }) => {
    await login.usernameInput.fill('bad@user.com');
    await login.passwordInput.fill('wrong');
    await login.submitButton.click();
    const snackbar = page.locator('mat-snack-bar-container');
    await expect(snackbar).toBeVisible();
    const bg = await snackbar.evaluate((el) => getComputedStyle(el.querySelector('.mdc-snackbar__surface')!).backgroundColor);
    expect(bg).toBe('rgb(244, 67, 54)');
  });

  test('error snackbar action label is a short dismiss label (not a sentence)', async ({ page }) => {
    await login.usernameInput.fill('bad@user.com');
    await login.passwordInput.fill('wrong');
    await login.submitButton.click();
    const snackbar = page.locator('mat-snack-bar-container');
    await expect(snackbar).toBeVisible();
    const actionBtn = snackbar.locator('button.mat-mdc-snack-bar-action');
    await expect(actionBtn).toBeVisible();
    const label = await actionBtn.innerText();
    expect(label.trim().length).toBeLessThan(15);
  });
});
