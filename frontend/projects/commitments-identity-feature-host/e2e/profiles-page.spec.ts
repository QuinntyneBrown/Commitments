import { test, expect } from '@playwright/test';
import { ProfilesPagePo } from './support/profiles-page.po';

test('fetches profiles list on load', async ({ page }) => {
  const pom = new ProfilesPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/profiles' }]);
});

test('renders one row per profile fixture', async ({ page }) => {
  const pom = new ProfilesPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture row
});
