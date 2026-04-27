import { chromium } from 'playwright';

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1920, height: 1080 } });
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
await page.locator('commitments-consistency-trend-tile').first().screenshot({ path: 'docs/bugs/screenshots/consistency-trend-tile-1920.png', timeout: 60000 });
const m = await page.locator('commitments-consistency-trend-tile .plot').first().evaluate(el => {
  const r = el.getBoundingClientRect();
  return { w: Math.round(r.width), h: Math.round(r.height) };
});
console.log('plot at xl:', JSON.stringify(m));
await browser.close();
