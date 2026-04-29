import { expect, test } from '@playwright/test';
import { TileHarnessPage } from './pages/tile-harness.page';
import { getBridge } from './support/bridge';
import { stubAllPluginEndpoints, stubJson } from './support/http-stub';
import { dailyResultsFixture } from './fixtures/daily-results.fixture';

test.describe('http recorder demo', () => {
  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
  });

  test('records daily-results GET and tile renders stubbed data', async ({ page }) => {
    await stubJson(page, /\/api\/v1\.0\/commitment\/daily-results/, dailyResultsFixture);
    const harness = new TileHarnessPage(page);
    await harness.goto('commitments.daily-results');

    const bridge = await getBridge(page);
    expect(bridge.http).toHaveLength(1);
    expect(bridge.http[0].method).toBe('GET');
    expect(bridge.http[0].url).toMatch(/daily-results/);
  });
});
