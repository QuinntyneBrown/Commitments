import { test, expect } from '@playwright/test';
import { BehavioursPagePo } from './support/behaviours-page.po';

test('fetches behaviours list on load', async ({ page }) => {
  const pom = new BehavioursPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/behaviours' }]);
});

test('renders one row per fixture behaviour', async ({ page }) => {
  const pom = new BehavioursPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});
