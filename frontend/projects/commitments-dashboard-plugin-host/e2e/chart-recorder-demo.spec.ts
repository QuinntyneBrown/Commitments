import { expect, test } from '@playwright/test';
import { TileHarnessPage } from './pages/tile-harness.page';
import { getBridge } from './support/bridge';
import { stubAllPluginEndpoints, stubJson } from './support/http-stub';
import { goalTrendFixture } from './fixtures/goal-trend.fixture';

test.describe('chart recorder demo', () => {
  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
  });

  test('records chart attach after consistency-trend tile loads', async ({ page }) => {
    await stubJson(page, /\/api\/v1\.0\/goal-progress\/trend/, goalTrendFixture);
    const harness = new TileHarnessPage(page);
    await harness.goto('commitments.consistency-trend');

    const bridge = await getBridge(page);
    const attachCalls = bridge.chart.filter((c) => c.kind === 'attach');
    expect(attachCalls.length).toBeGreaterThanOrEqual(1);
    expect(attachCalls[0]).toMatchObject({ kind: 'attach', type: 'line' });
  });
});
