import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../pages/tile-harness.page';
import { RelationsTilePage } from '../pages/relations-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../support/http-stub';
import { getBridge } from '../support/bridge';
import { relationsFixture } from '../fixtures/relations.fixture';

test.describe('relations tile', () => {
  let harness: TileHarnessPage;
  let tile: RelationsTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/relations\/summary/, relationsFixture);
    harness = new TileHarnessPage(page);
    tile = new RelationsTilePage(harness);
    await harness.goto('commitments.relations');
  });

  test('renders all relation rows', async () => {
    await expect(tile.rows).toHaveCount(3);
  });

  test('each row shows name and percentage', async () => {
    for (const [i, rel] of relationsFixture.relations.entries()) {
      await tile.expectRelation(i, rel.name, rel.percentage);
    }
  });

  test('issues GET to relations/summary', async ({ page }) => {
    const { http } = await getBridge(page);
    expect(http.find((c) => c.url.includes('relations/summary'))?.method).toBe('GET');
  });
});
