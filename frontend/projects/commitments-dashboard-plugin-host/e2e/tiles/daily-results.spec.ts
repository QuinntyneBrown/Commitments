import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../pages/tile-harness.page';
import { DailyResultsTilePage } from '../pages/daily-results-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../support/http-stub';
import { getBridge } from '../support/bridge';
import { dailyResultsFixture } from '../fixtures/daily-results.fixture';

test.describe('daily-results tile', () => {
  let harness: TileHarnessPage;
  let tile: DailyResultsTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/commitment\/daily-results/, dailyResultsFixture);
    harness = new TileHarnessPage(page);
    tile = new DailyResultsTilePage(harness);
    await harness.goto('commitments.daily-results');
  });

  test('renders fixture metric value', async () => {
    await tile.expectMetric(7, 12);
  });

  test('progressbar aria attributes match fixture', async () => {
    await tile.expectProgress(7, 12);
  });

  test('issues GET to commitment/daily-results', async ({ page }) => {
    const { http } = await getBridge(page);
    expect(http.find((c) => c.url.includes('commitment/daily-results'))?.method).toBe('GET');
  });
});
