import { Page } from '@playwright/test';
import { BackendCall, FeatureHarnessSnapshot } from '@commitments/dashboard-framework';
export abstract class BasePage {
  constructor(protected readonly page: Page) {}
  abstract get url(): string;
  async goto(): Promise<void> { await this.page.goto(this.url); }
  async bridge(): Promise<FeatureHarnessSnapshot> { return this.page.evaluate(() => (window as any).__featureHarness); }
  async backendCalls(): Promise<BackendCall[]> { return (await this.bridge()).backendCalls; }
}
