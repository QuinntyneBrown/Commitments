import { test, expect } from '@playwright/test';
import { SettingsPagePo } from './support/settings-page.po';

test('fetches current profile on load', async ({ page }) => {
  const pom = new SettingsPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/profiles/current' }]);
});

test('renders the fixture profile display name', async ({ page }) => {
  const pom = new SettingsPagePo(page);
  await pom.goto();
  await expect(pom.displayName).toHaveText('Test User');
});
