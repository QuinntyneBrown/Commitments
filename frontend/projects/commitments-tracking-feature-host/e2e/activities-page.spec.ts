import { test, expect } from '@playwright/test';
import { ActivitiesPagePo } from './support/activities-page.po';

test('fetches activities list on load', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/activities' }]);
});

test('renders one row per fixture activity', async ({ page }) => {
  const pom = new ActivitiesPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});
