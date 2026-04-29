import { test, expect } from '@playwright/test';
import { EditFrequencyPagePo } from './support/edit-frequency-page.po';

const FIXTURE_ID = '11111111-1111-1111-1111-111111111111';

test('fetches frequency when id is in route', async ({ page }) => {
  const pom = new EditFrequencyPagePo(page, FIXTURE_ID);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toContainEqual({ method: 'get', path: `api/v1.0/frequencies/${FIXTURE_ID}` });
});

test('does not fetch frequency when no id', async ({ page }) => {
  const pom = new EditFrequencyPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls.filter(c => c.method === 'get' && c.path.startsWith('api/v1.0/frequencies/'))).toHaveLength(0);
});

test('save fires POST to api/v1.0/frequencies', async ({ page }) => {
  const pom = new EditFrequencyPagePo(page);
  await pom.goto();
  await pom.nameInput.fill('3');
  await pom.save.click();
  const calls = await pom.backendCalls();
  expect(calls).toContainEqual(expect.objectContaining({ method: 'post', path: 'api/v1.0/frequencies' }));
});
