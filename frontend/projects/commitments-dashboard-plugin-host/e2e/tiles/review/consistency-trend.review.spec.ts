import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../../pages/tile-harness.page';
import { ConsistencyTrendTilePage } from '../../pages/consistency-trend-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../../support/http-stub';
import { getBridge } from '../../support/bridge';
import { goalTrendReviewFixture } from '../../fixtures/review/goal-trend.review.fixture';

test.describe('consistency-trend tile (review)', () => {
  let harness: TileHarnessPage;
  let tile: ConsistencyTrendTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/goal-progress\/trend/, goalTrendReviewFixture);
    harness = new TileHarnessPage(page);
    tile = new ConsistencyTrendTilePage(harness);
    await harness.goto('commitments.consistency-trend', {
      mode: 'review',
      asOf: '2026-03-28',
      params: { goalId: 'g-1', windowDays: '14' }
    });
  });

  test('shows REVIEW pill', async () => {
    await expect(tile.statusPillLabel).toHaveText('REVIEW');
  });

  test('passes asOf to backend', async ({ page }) => {
    const { mode, asOf, http } = await getBridge(page);
    expect(mode).toBe('review');
    expect(asOf).toBe('2026-03-28');
    const call = http.find((c) => c.url.includes('goal-progress/trend'));
    expect(call?.url).toContain('asOf=2026-03-28');
  });

  test('highlighted index shifts to asOf point in chart dataset', async ({ page }) => {
    const { chart } = await getBridge(page);
    const update = chart.find((c) => c.kind === 'updateDataset');
    expect(update).toBeDefined();
    expect((update as any).dataset.pointRadius[9]).toBe(7);   // asOf index
    expect((update as any).dataset.pointRadius[13]).toBe(3);  // last point, not highlighted
  });
});
