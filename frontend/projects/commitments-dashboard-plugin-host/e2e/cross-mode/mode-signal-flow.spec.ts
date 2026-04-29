import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../pages/tile-harness.page';
import { stubAllPluginEndpoints, stubJson } from '../support/http-stub';
import { getBridge } from '../support/bridge';
import { dailyResultsReviewFixture } from '../fixtures/review/daily-results.review.fixture';

test.describe('mode signal flow', () => {
  test('review navigation produces exactly one HTTP call with asOf', async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/commitment\/daily-results/, dailyResultsReviewFixture);
    const harness = new TileHarnessPage(page);
    await harness.goto('commitments.daily-results', { mode: 'review', asOf: '2026-04-01' });

    const { http } = await getBridge(page);
    const calls = http.filter((c) => c.url.includes('commitment/daily-results'));
    expect(calls).toHaveLength(1);
    expect(calls[0].url).toContain('asOf=2026-04-01');
  });
});
