import { chromium } from '@playwright/test';
import { mkdirSync } from 'node:fs';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const out = resolve(__dirname, '..', 'docs', 'bugs', 'screenshots');
mkdirSync(out, { recursive: true });

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1280, height: 900 } });
const page = await ctx.newPage();
const errors = [];
page.on('pageerror', (err) => errors.push(`pageerror: ${err.message}`));
page.on('console', (msg) => { if (msg.type() === 'error') errors.push(`console.error: ${msg.text()}`); });

try {
  await page.goto('http://127.0.0.1:4200/login', { waitUntil: 'domcontentloaded', timeout: 15000 });
} catch (e) {
  console.log(JSON.stringify({ navError: e.message }));
}
await page.waitForTimeout(2500);
await page.screenshot({ path: resolve(out, 'login-actual-1280.png'), fullPage: true });

const bodyBg = await page.evaluate(() => getComputedStyle(document.body).backgroundColor);
const url = page.url();
const title = await page.title();
const card = await page.locator('mat-card').first().evaluate((el) => el.getBoundingClientRect());
const submit = await page.locator('button[mat-raised-button]').first().evaluate((el) => el.getBoundingClientRect()).catch(() => null);
console.log(JSON.stringify({ url, title, bodyBg, errors, card, submit }, null, 2));
await browser.close();
