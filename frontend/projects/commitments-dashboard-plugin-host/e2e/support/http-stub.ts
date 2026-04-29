import { Page } from '@playwright/test';

export async function stubJson(
  page: Page,
  pattern: string | RegExp,
  body: unknown,
  init: { status?: number } = {}
): Promise<void> {
  await page.route(pattern, async (route) => {
    await route.fulfill({
      status: init.status ?? 200,
      contentType: 'application/json',
      body: JSON.stringify(body)
    });
  });
}

export async function stubAllPluginEndpoints(page: Page): Promise<void> {
  await page.route(/\/api\/v1\.0\/.*/, async (route) =>
    route.fulfill({ status: 200, contentType: 'application/json', body: '{}' })
  );
}
