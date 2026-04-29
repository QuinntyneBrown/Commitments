import { expect, Locator } from '@playwright/test';
import { TileHarnessPage } from './tile-harness.page';

export class WeeklyFocusTilePage {
  constructor(private readonly harness: TileHarnessPage) {}

  get root(): Locator { return this.harness.page.getByTestId('weekly-focus-tile'); }
  get focusRows(): Locator { return this.root.locator('.focus-list li'); }

  async expectFocusArea(index: number, name: string, metric: string): Promise<void> {
    const row = this.focusRows.nth(index);
    await expect(row.locator('strong')).toHaveText(name);
    await expect(row.locator('span')).toHaveText(metric);
  }
}
