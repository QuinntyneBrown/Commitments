import { test, expect } from '@playwright/test';
import { EditNotePagePo } from './support/edit-note-page.po';

test('deep-link with slug fetches that note', async ({ page }) => {
  const pom = new EditNotePagePo(page, 'getting-started');
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/notes/slug/getting-started' }]);
});
