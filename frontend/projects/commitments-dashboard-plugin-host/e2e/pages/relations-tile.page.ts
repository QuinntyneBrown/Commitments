import { expect, Locator } from '@playwright/test';
import { TileHarnessPage } from './tile-harness.page';

export class RelationsTilePage {
  constructor(private readonly harness: TileHarnessPage) {}

  get root(): Locator { return this.harness.page.getByTestId('relations-tile'); }
  get rows(): Locator { return this.root.locator('.relations__row'); }

  async expectRelation(index: number, name: string, percentage: number): Promise<void> {
    const row = this.rows.nth(index);
    await expect(row.locator('span')).toHaveText(name);
    await expect(row.locator('strong')).toHaveText(`${percentage}%`);
  }
}
