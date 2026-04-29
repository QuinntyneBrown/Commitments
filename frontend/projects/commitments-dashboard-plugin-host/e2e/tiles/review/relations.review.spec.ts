import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../../pages/tile-harness.page';
import { RelationsTilePage } from '../../pages/relations-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../../support/http-stub';
import { getBridge } from '../../support/bridge';
import { relationsReviewFixture } from '../../fixtures/review/relations.review.fixture';

test.describe('relations tile (review)', () => {
  let harness: TileHarnessPage;
  let tile: RelationsTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/relations\/summary/, relationsReviewFixture);
    harness = new TileHarnessPage(page);
    tile = new RelationsTilePage(harness);
    await harness.goto('commitments.relations', { mode: 'review', asOf: '2026-04-01' });
  });

  test('renders fixture relation rows', async () => {
    await tile.expectRelation(0, relationsReviewFixture.relations[0].name, relationsReviewFixture.relations[0].percentage);
    await tile.expectRelation(1, relationsReviewFixture.relations[1].name, relationsReviewFixture.relations[1].percentage);
  });

  test('passes asOf to backend', async ({ page }) => {
    const { mode, asOf, http } = await getBridge(page);
    expect(mode).toBe('review');
    expect(asOf).toBe('2026-04-01');
    const call = http.find((c) => c.url.includes('relations/summary'));
    expect(call?.url).toContain('asOf=2026-04-01');
  });
});
