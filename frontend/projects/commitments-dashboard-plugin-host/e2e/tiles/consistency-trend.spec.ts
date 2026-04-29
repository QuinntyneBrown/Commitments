import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../pages/tile-harness.page';
import { ConsistencyTrendTilePage } from '../pages/consistency-trend-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../support/http-stub';
import { getBridge } from '../support/bridge';
import { goalTrendFixture } from '../fixtures/goal-trend.fixture';

test.describe('consistency-trend tile', () => {
  let harness: TileHarnessPage;
  let tile: ConsistencyTrendTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/goal-progress\/trend/, goalTrendFixture);
    harness = new TileHarnessPage(page);
    tile = new ConsistencyTrendTilePage(harness);
    await harness.goto('commitments.consistency-trend', {
      params: { goalId: 'g-1', windowDays: '14' }
    });
  });

  test('renders canvas with non-zero dimensions', async () => {
    await tile.expectCanvasVisible();
  });

  test('renders current percentage and peak/low from fixture', async () => {
    await expect(tile.currentPercent).toHaveText(`${goalTrendFixture.currentPercentage}%`);
    await expect(tile.subCaption).toContainText(`Peak ${goalTrendFixture.peakPercentage}%`);
    await expect(tile.subCaption).toContainText(`Low ${goalTrendFixture.lowPercentage}%`);
  });

  test('chart bridge records attach with line type and updateDataset with fixture data', async ({ page }) => {
    const { chart } = await getBridge(page);
    const attach = chart.find((c) => c.kind === 'attach');
    expect(attach).toBeDefined();
    expect((attach as any).type).toBe('line');

    const update = chart.find((c) => c.kind === 'updateDataset');
    expect(update).toBeDefined();
    expect((update as any).dataset.data).toEqual(
      goalTrendFixture.points.map((p) => p.percentage)
    );
  });

  test('issues GET to goal-progress/trend', async ({ page }) => {
    const { http } = await getBridge(page);
    const call = http.find((c) => c.url.includes('goal-progress/trend'));
    expect(call?.method).toBe('GET');
  });
});
