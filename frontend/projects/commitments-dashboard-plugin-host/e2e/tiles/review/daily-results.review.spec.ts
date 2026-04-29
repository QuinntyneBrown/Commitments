import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../../pages/tile-harness.page';
import { DailyResultsTilePage } from '../../pages/daily-results-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../../support/http-stub';
import { getBridge } from '../../support/bridge';
import { dailyResultsReviewFixture } from '../../fixtures/review/daily-results.review.fixture';

test.describe('daily-results tile (review)', () => {
  let harness: TileHarnessPage;
  let tile: DailyResultsTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/commitment\/daily-results/, dailyResultsReviewFixture);
    harness = new TileHarnessPage(page);
    tile = new DailyResultsTilePage(harness);
    await harness.goto('commitments.daily-results', { mode: 'review', asOf: '2026-04-01' });
  });

  test('shows REVIEW pill', async () => {
    await expect(tile.statusPillLabel).toHaveText('REVIEW');
  });

  test('renders fixture metric values', async () => {
    await tile.expectMetric(dailyResultsReviewFixture.completed, dailyResultsReviewFixture.total);
  });

  test('passes asOf to backend', async ({ page }) => {
    const { mode, asOf, http } = await getBridge(page);
    expect(mode).toBe('review');
    expect(asOf).toBe('2026-04-01');
    const call = http.find((c) => c.url.includes('commitment/daily-results'));
    expect(call?.url).toContain('asOf=2026-04-01');
  });
});
