import { expect, Locator } from '@playwright/test';
import { TileHarnessPage } from './tile-harness.page';

export class DailyResultsTilePage {
  constructor(private readonly harness: TileHarnessPage) {}

  get root(): Locator { return this.harness.page.getByTestId('daily-results-tile'); }
  get metricValue(): Locator { return this.root.locator('.metric__value'); }
  get progressBar(): Locator { return this.root.locator('[role="progressbar"]'); }

  async expectMetric(completed: number, total: number): Promise<void> {
    await expect(this.metricValue).toHaveText(`${completed} / ${total}`);
  }

  async expectProgress(completed: number, total: number): Promise<void> {
    await expect(this.progressBar).toHaveAttribute('aria-valuenow', String(completed));
    await expect(this.progressBar).toHaveAttribute('aria-valuemax', String(total));
  }
}
