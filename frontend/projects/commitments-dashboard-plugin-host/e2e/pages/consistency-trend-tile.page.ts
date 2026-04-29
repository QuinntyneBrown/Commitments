import { expect, Locator } from '@playwright/test';
import { TileHarnessPage } from './tile-harness.page';

export class ConsistencyTrendTilePage {
  constructor(private readonly harness: TileHarnessPage) {}

  get root(): Locator { return this.harness.page.getByTestId('consistency-trend-tile'); }
  get canvas(): Locator { return this.harness.page.getByTestId('consistency-trend-canvas'); }
  get currentPercent(): Locator { return this.root.locator('.metric-header__value'); }
  get subCaption(): Locator { return this.root.locator('.metric-header__sub-caption'); }
  get deltaValue(): Locator { return this.root.locator('.delta-badge__value'); }
  get statusPillLabel(): Locator { return this.root.locator('.status-pill__label'); }

  async expectCanvasVisible(): Promise<void> {
    await expect(this.canvas).toBeVisible();
    const box = await this.canvas.boundingBox();
    expect(box?.width ?? 0).toBeGreaterThan(0);
    expect(box?.height ?? 0).toBeGreaterThan(0);
  }
}
