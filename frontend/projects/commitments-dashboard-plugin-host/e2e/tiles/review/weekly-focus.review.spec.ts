import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../../pages/tile-harness.page';
import { WeeklyFocusTilePage } from '../../pages/weekly-focus-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../../support/http-stub';
import { getBridge } from '../../support/bridge';
import { weeklyFocusReviewFixture } from '../../fixtures/review/weekly-focus.review.fixture';

test.describe('weekly-focus tile (review)', () => {
  let harness: TileHarnessPage;
  let tile: WeeklyFocusTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/weekly-focus/, weeklyFocusReviewFixture);
    harness = new TileHarnessPage(page);
    tile = new WeeklyFocusTilePage(harness);
    await harness.goto('commitments.weekly-focus', { mode: 'review', asOf: '2026-04-01' });
  });

  test('renders fixture focus areas', async () => {
    await tile.expectFocusArea(0, weeklyFocusReviewFixture.focusAreas[0].name, weeklyFocusReviewFixture.focusAreas[0].supportingMetric);
    await tile.expectFocusArea(1, weeklyFocusReviewFixture.focusAreas[1].name, weeklyFocusReviewFixture.focusAreas[1].supportingMetric);
  });

  test('passes asOf to backend', async ({ page }) => {
    const { mode, asOf, http } = await getBridge(page);
    expect(mode).toBe('review');
    expect(asOf).toBe('2026-04-01');
    const call = http.find((c) => c.url.includes('weekly-focus'));
    expect(call?.url).toContain('asOf=2026-04-01');
  });
});
