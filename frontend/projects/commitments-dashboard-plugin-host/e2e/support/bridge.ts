import { Page } from '@playwright/test';
import { PluginHarnessSnapshot } from '../../src/app/harness/window-bridge.service';

export type { PluginHarnessSnapshot };

export async function getBridge(page: Page): Promise<PluginHarnessSnapshot> {
  return page.evaluate(() => (window as any).__pluginHarness);
}

export async function getBridgeMode(page: Page): Promise<'live' | 'review'> {
  return (await getBridge(page)).mode;
}
