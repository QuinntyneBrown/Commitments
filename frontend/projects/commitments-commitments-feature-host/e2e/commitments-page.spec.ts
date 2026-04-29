import { test, expect } from '@playwright/test';
import { CommitmentsPagePo } from './support/commitments-page.po';

test('fetches commitments list on load', async ({ page }) => {
  const pom = new CommitmentsPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/commitments' }]);
});

test('renders one row per fixture commitment', async ({ page }) => {
  const pom = new CommitmentsPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});
