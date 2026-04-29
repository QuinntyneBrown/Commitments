import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../../pages/tile-harness.page';
import { OutstandingTodosTilePage } from '../../pages/outstanding-todos-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../../support/http-stub';
import { getBridge } from '../../support/bridge';
import { outstandingTodosReviewFixture } from '../../fixtures/review/outstanding-todos.review.fixture';

test.describe('outstanding-todos tile (review)', () => {
  let harness: TileHarnessPage;
  let tile: OutstandingTodosTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/todos\/outstanding-count/, outstandingTodosReviewFixture);
    harness = new TileHarnessPage(page);
    tile = new OutstandingTodosTilePage(harness);
    await harness.goto('commitments.outstanding-todos', { mode: 'review', asOf: '2026-04-01' });
  });

  test('shows review pill variant', async () => {
    await expect(tile.reviewPill).toBeVisible();
  });

  test('renders fixture count', async () => {
    await tile.expectCount(outstandingTodosReviewFixture.count);
  });

  test('passes asOf and includeToday to backend', async ({ page }) => {
    const { mode, asOf, http } = await getBridge(page);
    expect(mode).toBe('review');
    expect(asOf).toBe('2026-04-01');
    const call = http.find((c) => c.url.includes('todos/outstanding-count'));
    expect(call?.url).toContain('asOf=2026-04-01');
    expect(call?.url).toContain('includeToday=true');
  });
});
