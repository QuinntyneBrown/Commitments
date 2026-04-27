import { chromium } from 'playwright';

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1280, height: 800 } });
const page = await ctx.newPage();
await page.goto('http://127.0.0.1:4200/', { waitUntil: 'domcontentloaded' });
await page.waitForTimeout(1500);
await page.getByTestId('add-tile-fab').click();
await page.waitForTimeout(500);
await page.locator('.add-tile-dialog__cell', { hasText: 'Live Goal Metrics' }).click();
await page.waitForTimeout(200);
await page.getByTestId('add-tile-dialog-confirm').click();
await page.waitForTimeout(800);
await page.locator('[data-testid=live-goal-metrics-tile]').screenshot({ path: 'docs/bugs/screenshots/live-goal-metrics-tile-1280.png' });
await browser.close();
console.log('done');
