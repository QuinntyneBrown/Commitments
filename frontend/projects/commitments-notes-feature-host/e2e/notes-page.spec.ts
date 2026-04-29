import { test, expect } from '@playwright/test';
import { NotesPagePo } from './support/notes-page.po';

test('fetches notes list on load', async ({ page }) => {
  const pom = new NotesPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/notes' }]);
});

test('renders one row per fixture note', async ({ page }) => {
  const pom = new NotesPagePo(page);
  await pom.goto();
  await expect(pom.rows).toHaveCount(2); // 1 header + 1 fixture
});
