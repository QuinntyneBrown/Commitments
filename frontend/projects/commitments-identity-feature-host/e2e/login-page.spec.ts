import { test, expect } from '@playwright/test';
import { LoginPagePo } from './support/login-page.po';

test('submits credentials to api/v1.0/users/token', async ({ page }) => {
  const pom = new LoginPagePo(page);
  await pom.goto();
  await pom.signIn('alice', 'pw');
  const calls = await pom.backendCalls();
  expect(calls).toEqual([
    { method: 'post', path: 'api/v1.0/users/token', body: { username: 'alice', password: 'pw' } }
  ]);
});
