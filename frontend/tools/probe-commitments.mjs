import { chromium } from 'playwright';

const BASE = process.env.BASE_URL ?? 'http://127.0.0.1:4200';
const ROUTE = process.env.ROUTE ?? '/commitments';
const SCREENSHOT = process.env.SCREENSHOT ?? `docs/bugs/screenshots/${ROUTE.replace(/^\//, '') || 'index'}-actual-1280.png`;

const browser = await chromium.launch();
const ctx = await browser.newContext({ viewport: { width: 1280, height: 800 } });
const page = await ctx.newPage();

const consoleMsgs = [];
page.on('console', m => consoleMsgs.push(`[${m.type()}] ${m.text()}`));
page.on('pageerror', e => consoleMsgs.push(`[pageerror] ${e.message}`));
page.on('requestfailed', r => consoleMsgs.push(`[reqfail] ${r.url()} ${r.failure()?.errorText}`));

await page.goto(BASE + ROUTE, { waitUntil: 'domcontentloaded' });
await page.waitForTimeout(1500);

const heading = await page.locator('h1, h2, app-primary-header').first().textContent().catch(() => null);
const url = page.url();

const grid = await page.locator('ag-grid-angular').count();
const fab = await page.locator('button.mat-mdc-fab, button.mat-fab').count();
const fabIcon = await page.locator('button.mat-mdc-fab mat-icon, button.mat-fab mat-icon').first().textContent().catch(() => null);
const sidenav = await page.locator('mat-sidenav, app-sidenav').count();
const topbar = await page.locator('mat-toolbar, app-primary-header').count();

const bodyBg = await page.evaluate(() => getComputedStyle(document.body).backgroundColor);
const gridTheme = await page.evaluate(() => {
  const el = document.querySelector('ag-grid-angular');
  return el ? Array.from(el.classList) : [];
});
const gridBg = await page.locator('.ag-root-wrapper, .ag-theme-material').first()
  .evaluate(el => el ? getComputedStyle(el).backgroundColor : 'none').catch(() => 'none');

await page.screenshot({ path: SCREENSHOT, fullPage: true });

const pageHtml = await page.locator('app-commitments-page').first().innerHTML().catch(() => '<no app-commitments-page>');
const rootCount = await page.locator('app-commitments-page').count();
const routerOutletSiblingTag = await page.evaluate(() => {
  const ro = document.querySelector('router-outlet');
  if (!ro) return null;
  const sib = ro.nextElementSibling;
  return sib ? sib.tagName + (sib.id ? `#${sib.id}` : '') + (sib.className ? `.${sib.className.split(' ').join('.')}` : '') : null;
});

console.log(JSON.stringify({
  url, heading,
  counts: { grid, fab, sidenav, topbar, root: rootCount },
  fabIcon: fabIcon?.trim() ?? null,
  bodyBg, gridTheme, gridBg,
  routerOutletSiblingTag,
  pageHtmlLen: pageHtml.length,
  pageHtmlFirst1k: pageHtml.slice(0, 1000),
  consoleSample: consoleMsgs.slice(0, 30),
}, null, 2));

await browser.close();
