import { test, expect } from '@playwright/test';
import { ToDosPagePo } from './support/to-dos-page.po';

test('fetches to-dos list on load', async ({ page }) => {
  const pom = new ToDosPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/toDos' }]);
});

test('renders one row per fixture to-do', async ({ page }) => {
  const pom = new ToDosPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});
