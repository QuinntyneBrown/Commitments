import { chromium, type Page } from '@playwright/test';
import { mkdirSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const SCRIPT_DIR = dirname(fileURLToPath(import.meta.url));
const TARGET_URL = process.env.TOUR_URL ?? 'http://127.0.0.1:4200/';
const OUTPUT_DIR = join(SCRIPT_DIR, 'recordings');
const VIEWPORT = { width: 1280, height: 800 };
const TARGET_DURATION_MS = 40_000;

mkdirSync(OUTPUT_DIR, { recursive: true });

async function pause(ms: number) {
  await new Promise((r) => setTimeout(r, ms));
}

async function addTile(page: Page, tileId: string) {
  await page.getByTestId('add-tile-fab').click();
  const cell = page.getByTestId(`add-tile-cell-${tileId}`);
  await cell.waitFor({ state: 'visible', timeout: 5_000 });
  await cell.click();
  await page.getByTestId('add-tile-dialog-confirm').click();
  await page.getByTestId('add-tile-dialog').waitFor({ state: 'detached', timeout: 5_000 }).catch(() => undefined);
}

async function main() {
  const startedAt = Date.now();
  const browser = await chromium.launch({ headless: true });
  const context = await browser.newContext({
    viewport: VIEWPORT,
    recordVideo: { dir: OUTPUT_DIR, size: VIEWPORT },
  });
  const page = await context.newPage();
  page.on('pageerror', (e) => console.log('[pageerror]', e.message));
  page.on('console', (m) => {
    if (m.type() === 'error') console.log('[console.error]', m.text());
  });

  const elapsed = () => Date.now() - startedAt;
  const remaining = () => TARGET_DURATION_MS - elapsed();

  await page.goto(TARGET_URL, { waitUntil: 'networkidle' });

  await page.getByTestId('dashboard-shell').waitFor({ state: 'visible', timeout: 30_000 });
  await pause(1500);

  // Add the Consistency Trend (Chart.js line graph) tile via the new FAB + dialog flow.
  await addTile(page, 'commitments.consistency-trend');

  const trendTile = page
    .getByTestId('tile-shell')
    .filter({ hasText: 'Consistency Trend' })
    .first();
  await trendTile.waitFor({ state: 'visible', timeout: 10_000 });
  const canvas = page.getByTestId('consistency-trend-canvas').first();
  await canvas.waitFor({ state: 'visible', timeout: 10_000 });

  // Linger so the line draws + animates.
  await trendTile.scrollIntoViewIfNeeded();
  await pause(2500);
  await canvas.hover({ position: { x: 60, y: 100 }, force: true });
  await pause(900);
  await canvas.hover({ position: { x: 220, y: 80 }, force: true });
  await pause(900);
  await canvas.hover({ position: { x: 380, y: 120 }, force: true });
  await pause(900);
  await canvas.hover({ position: { x: 520, y: 90 }, force: true });
  await pause(1200);

  // Add a second Consistency Trend so two line graphs render side by side.
  await addTile(page, 'commitments.consistency-trend');
  await pause(800);

  // Add a Live Goal Metrics tile for variety.
  await addTile(page, 'commitments.live-goal-metrics');
  await pause(1200);

  // Tour the second chart so the recording shows two simultaneous line graphs.
  const allCanvases = page.getByTestId('consistency-trend-canvas');
  const second = allCanvases.nth(1);
  if (await second.count()) {
    await second.scrollIntoViewIfNeeded();
    for (const x of [60, 200, 360, 500, 280]) {
      if (remaining() < 4000) break;
      await second.hover({ position: { x, y: 95 }, force: true }).catch(() => {});
      await pause(700);
    }
  }

  // Burn any remaining time hovering across the chart so the recording stays animated.
  while (remaining() > 1500) {
    const slot = remaining();
    const x = 60 + ((slot / 200) % 1) * 480;
    await canvas.hover({ position: { x, y: 70 + ((slot / 100) % 1) * 80 }, force: true }).catch(() => {});
    await pause(700);
  }

  await pause(Math.max(0, remaining()));

  const video = page.video();
  await context.close();
  const path = video ? await video.path() : null;
  await browser.close();

  if (path) {
    console.log(`VIDEO_PATH=${path}`);
    console.log(`DURATION_MS=${Date.now() - startedAt}`);
  } else {
    console.error('No video produced');
    process.exit(1);
  }
}

main().catch((e) => {
  console.error(e);
  process.exit(1);
});
