import { test, expect } from '@playwright/test';
import { BehaviourTypesPagePo } from './support/behaviour-types-page.po';

test('fetches behaviour types list on load', async ({ page }) => {
  const pom = new BehaviourTypesPagePo(page);
  await pom.goto();
  const calls = await pom.backendCalls();
  expect(calls).toEqual([{ method: 'get', path: 'api/v1.0/behaviourTypes' }]);
});
