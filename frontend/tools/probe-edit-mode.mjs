import { chromium } from 'playwright';

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1280, height: 800 } });
const page = await ctx.newPage();
await page.goto('http://127.0.0.1:4200/', { waitUntil: 'domcontentloaded' });
await page.waitForTimeout(1500);
await page.getByTestId('edit-mode-enter').click();
await page.waitForTimeout(800);
await page.screenshot({ path: 'docs/bugs/screenshots/dashboard-edit-mode-1280.png', fullPage: false });
await browser.close();
console.log('done');
