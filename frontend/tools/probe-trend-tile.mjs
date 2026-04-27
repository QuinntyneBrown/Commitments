import { chromium } from 'playwright';

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1280, height: 800 } });
const page = await ctx.newPage();
await page.addInitScript(() => localStorage.clear());
await page.goto('http://127.0.0.1:4200/', { waitUntil: 'domcontentloaded' });
await page.waitForTimeout(1500);
await page.getByTestId('add-tile-fab').click();
await page.waitForTimeout(500);
await page.locator('.add-tile-dialog__cell', { hasText: 'Consistency Trend' }).click();
await page.waitForTimeout(200);
await page.getByTestId('add-tile-dialog-confirm').click();
await page.waitForTimeout(2500);
const tile = page.locator('commitments-consistency-trend-tile, [data-tile-id="commitments.consistency-trend"]').first();
const count = await tile.count();
console.log('tile count:', count);
if (count > 0) {
  await tile.screenshot({ path: 'docs/bugs/screenshots/consistency-trend-tile-1280.png', timeout: 60000 });
}
await browser.close();
