import { test, expect } from '@playwright/test';
import { EditFrequencyPagePo } from './support/edit-frequency-page.po';

test('deep-link with id fetches that frequency', async ({ page }) => {
  const pom = new EditFrequencyPagePo(page, '1');
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/frequencies/1' }]);
});

test('new route makes no GET call', async ({ page }) => {
  const pom = new EditFrequencyPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([]);
});
