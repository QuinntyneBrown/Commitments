import { expect, Locator, Page } from '@playwright/test';

export class TileHarnessPage {
  constructor(public readonly page: Page) {}

  async goto(tileId: string, opts: { mode?: 'live' | 'review'; asOf?: string } = {}) {
    const params = new URLSearchParams();
    if (opts.mode) params.set('mode', opts.mode);
    if (opts.asOf) params.set('asOf', opts.asOf);
    const qs = params.toString();
    await this.page.goto(`/tile/${tileId}${qs ? '?' + qs : ''}`);
    await expect(this.page.getByTestId('tile-shell')).toBeVisible();
  }

  tile(): Locator {
    return this.page.getByTestId('tile-shell');
  }

  tileNotFound(): Locator {
    return this.page.getByTestId('tile-not-found');
  }
}
