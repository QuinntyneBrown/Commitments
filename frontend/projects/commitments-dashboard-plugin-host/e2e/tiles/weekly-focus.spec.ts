import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../pages/tile-harness.page';
import { WeeklyFocusTilePage } from '../pages/weekly-focus-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../support/http-stub';
import { getBridge } from '../support/bridge';
import { weeklyFocusFixture } from '../fixtures/weekly-focus.fixture';

test.describe('weekly-focus tile', () => {
  let harness: TileHarnessPage;
  let tile: WeeklyFocusTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/weekly-focus/, weeklyFocusFixture);
    harness = new TileHarnessPage(page);
    tile = new WeeklyFocusTilePage(harness);
    await harness.goto('commitments.weekly-focus');
  });

  test('renders all 3 focus area rows', async () => {
    await expect(tile.focusRows).toHaveCount(3);
  });

  test('each focus row shows name and supporting metric', async () => {
    for (const [i, area] of weeklyFocusFixture.focusAreas.entries()) {
      await tile.expectFocusArea(i, area.name, area.supportingMetric);
    }
  });

  test('issues GET to weekly-focus', async ({ page }) => {
    const { http } = await getBridge(page);
    expect(http.find((c) => c.url.includes('weekly-focus'))?.method).toBe('GET');
  });
});
