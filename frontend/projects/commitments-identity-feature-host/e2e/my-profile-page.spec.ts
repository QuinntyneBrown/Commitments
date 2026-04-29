import { test, expect } from '@playwright/test';
import { MyProfilePagePo } from './support/my-profile-page.po';

test('fetches current profile on load', async ({ page }) => {
  const pom = new MyProfilePagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/profiles/current' }]);
});
