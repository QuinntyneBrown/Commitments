import { chromium } from 'playwright';

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1280, height: 800 } });
const page = await ctx.newPage();
await page.addInitScript(() => localStorage.clear());
await page.goto('http://127.0.0.1:4200/', { waitUntil: 'domcontentloaded' });
await page.waitForTimeout(2000);
const tile = page.locator('[data-tile-id="commitments.monthly-progress"]').first();
const count = await tile.count();
console.log('tile count:', count);
if (count > 0) {
  await tile.screenshot({ path: 'docs/bugs/screenshots/monthly-progress-tile-1280.png', timeout: 60000 });
}
await browser.close();
