import { chromium } from 'playwright';

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1280, height: 800 } });
const page = await ctx.newPage();
await page.addInitScript(() => localStorage.clear());
await page.goto('http://127.0.0.1:4200/', { waitUntil: 'domcontentloaded' });
await page.waitForTimeout(1500);
await page.locator('.mode-toggle__segment--review').click();
await page.waitForTimeout(800);
await page.locator('[data-testid=dashboard-mode-toggle]').screenshot({ path: 'docs/bugs/screenshots/mode-toggle-review-1280.png' });
await page.screenshot({ path: 'docs/bugs/screenshots/dashboard-review-1280.png', fullPage: false });
await browser.close();
console.log('done');
