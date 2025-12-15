import { test, expect } from '@playwright/test';
import { HomePage } from './pages/home.page';

test.describe('Commitments Application', () => {
  let homePage: HomePage;

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page);
  });

  test('should display the application', async ({ page }) => {
    await homePage.goto();
    
    // Verify the page loads
    await expect(page).toHaveURL(/\//);
    
    // Verify basic page structure
    await homePage.waitForPageLoad();
  });

  test('should have a title', async ({ page }) => {
    await homePage.goto();
    const title = await homePage.getTitle();
    
    // Verify title is not empty
    expect(title).toBeTruthy();
  });
});
