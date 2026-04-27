import { chromium } from '@playwright/test';
import { mkdirSync } from 'node:fs';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const out = resolve(__dirname, '..', 'docs', 'bugs', 'screenshots');
mkdirSync(out, { recursive: true });

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 768, height: 1024 } });
const page = await ctx.newPage();
const errors = [];
page.on('pageerror', (err) => errors.push(`pageerror: ${err.message}`));
page.on('console', (msg) => { if (msg.type() === 'error') errors.push(`console.error: ${msg.text()}`); });

try {
  await page.goto('http://127.0.0.1:4200/', { waitUntil: 'domcontentloaded', timeout: 15000 });
} catch (e) {
  console.log(JSON.stringify({ navError: e.message }));
}
await page.waitForTimeout(2500);
await page.screenshot({ path: resolve(out, 'dashboard-actual-768.png'), fullPage: false });

const url = page.url();
const title = await page.title();
const bodyBg = await page.evaluate(() => getComputedStyle(document.body).backgroundColor);
const shell = await page.locator('[data-testid="dashboard-shell"]').first().evaluate((el) => {
  const r = el.getBoundingClientRect();
  return { width: r.width, height: r.height, bg: getComputedStyle(el).backgroundColor };
}).catch(() => null);
const topbar = await page.locator('.dashboard-shell__topbar').first().evaluate((el) => ({
  bg: getComputedStyle(el).backgroundColor,
  height: el.getBoundingClientRect().height,
})).catch(() => null);
const modeToggle = await page.locator('.mode-toggle').first().evaluate((el) => ({
  bg: getComputedStyle(el).backgroundColor,
  border: getComputedStyle(el).borderColor,
})).catch(() => null);
const inactiveSegment = await page.locator('.mode-toggle__segment:not(.mode-toggle__segment--active)').first()
  .evaluate((el) => ({
    bg: getComputedStyle(el).backgroundColor,
    color: getComputedStyle(el).color,
  })).catch(() => null);
const reviewToggle = page.locator('.mode-toggle__segment--review');
if (await reviewToggle.count() > 0) {
  await reviewToggle.click();
  await page.waitForTimeout(800);
  await page.screenshot({ path: resolve(out, 'dashboard-review-actual-1280.png'), fullPage: false });
  // back to live
  await page.locator('.mode-toggle__segment--live').click();
  await page.waitForTimeout(400);
}

const select = page.locator('[data-testid="tile-select"]');
await select.selectOption({ label: 'Monthly Progress' });
await page.locator('[data-testid="add-tile"]').click();
await page.waitForTimeout(300);
const storage = await page.evaluate(() => {
  const out = {};
  for (let i = 0; i < localStorage.length; i++) {
    const k = localStorage.key(i);
    out[k] = localStorage.getItem(k);
  }
  return out;
});

console.log(JSON.stringify({ url, title, bodyBg, shell, topbar, modeToggle, inactiveSegment, storage, errors }, null, 2));
await browser.close();
