import { chromium } from 'playwright';

const VW = parseInt(process.env.VW ?? '1280', 10);
const VH = parseInt(process.env.VH ?? '800', 10);

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: VW, height: VH } });
const page = await ctx.newPage();
await page.goto('http://127.0.0.1:4200/', { waitUntil: 'domcontentloaded' });
await page.waitForTimeout(2000);

const tiles = await page.locator('[data-testid=dashboard-tile]').evaluateAll((els) =>
  els.map((el) => {
    const r = el.getBoundingClientRect();
    return { w: Math.round(r.width), h: Math.round(r.height), x: Math.round(r.x), y: Math.round(r.y) };
  })
);

const grid = await page.locator('.dashboard-grid').first().evaluate((el) => {
  const r = el.getBoundingClientRect();
  return { w: Math.round(r.width), h: Math.round(r.height) };
});

console.log(JSON.stringify({ viewport: { VW, VH }, grid, tiles }, null, 2));
await browser.close();
