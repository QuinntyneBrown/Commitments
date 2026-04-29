import { expect, test } from '@playwright/test';
import { TileHarnessPage } from '../pages/tile-harness.page';
import { OutstandingTodosTilePage } from '../pages/outstanding-todos-tile.page';
import { stubAllPluginEndpoints, stubJson } from '../support/http-stub';
import { getBridge } from '../support/bridge';
import { outstandingTodosFixture } from '../fixtures/outstanding-todos.fixture';

test.describe('outstanding-todos tile', () => {
  let harness: TileHarnessPage;
  let tile: OutstandingTodosTilePage;

  test.beforeEach(async ({ page }) => {
    await stubAllPluginEndpoints(page);
    await stubJson(page, /\/todos\/outstanding-count/, outstandingTodosFixture);
    harness = new TileHarnessPage(page);
    tile = new OutstandingTodosTilePage(harness);
    await harness.goto('commitments.outstanding-todos');
  });

  test('renders fixture count', async () => {
    await tile.expectCount(4);
  });

  test('issues GET to todos/outstanding-count', async ({ page }) => {
    const { http } = await getBridge(page);
    expect(http.find((c) => c.url.includes('todos/outstanding-count'))?.method).toBe('GET');
  });
});
