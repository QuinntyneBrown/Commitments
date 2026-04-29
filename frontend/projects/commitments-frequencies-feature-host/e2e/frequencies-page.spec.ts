import { test, expect } from '@playwright/test';
import { FrequenciesPagePo } from './support/frequencies-page.po';

test('fetches frequencies list on load', async ({ page }) => {
  const pom = new FrequenciesPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/frequencies' }]);
});

test('renders one row per fixture frequency', async ({ page }) => {
  const pom = new FrequenciesPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});

test('edit link navigates to edit-frequency page', async ({ page }) => {
  const pom = new FrequenciesPagePo(page);
  await pom.goto();
  await pom.editLinks.first().click();
  await expect(page).toHaveURL(/\/edit-frequency\//);
});
