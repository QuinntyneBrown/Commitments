import { test, expect } from '@playwright/test';
import { CardsPagePo } from './support/cards-page.po';

test('fetches cards list on load', async ({ page }) => {
  const pom = new CardsPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/cards' }]);
});

test('renders one row per fixture card', async ({ page }) => {
  const pom = new CardsPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});
