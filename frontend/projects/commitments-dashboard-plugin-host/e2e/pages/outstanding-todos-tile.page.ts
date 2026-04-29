import { expect, Locator } from '@playwright/test';
import { TileHarnessPage } from './tile-harness.page';

export class OutstandingTodosTilePage {
  constructor(private readonly harness: TileHarnessPage) {}

  get root(): Locator { return this.harness.page.getByTestId('outstanding-todos-tile'); }
  get count(): Locator { return this.root.locator('.todo-count'); }
  get reviewPill(): Locator { return this.root.locator('.status-pill--review'); }

  async expectCount(n: number): Promise<void> {
    await expect(this.count).toHaveText(String(n));
  }
}
