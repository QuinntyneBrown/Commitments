import { expect, Locator, Page } from '@playwright/test';

export interface GotoOpts {
  mode?: 'live' | 'review';
  asOf?: string;
  params?: Record<string, string>;
}

export class TileHarnessPage {
  constructor(public readonly page: Page) {}

  async goto(tileId: string, opts: GotoOpts = {}) {
    const search = new URLSearchParams();
    if (opts.mode) search.set('mode', opts.mode);
    if (opts.asOf) search.set('asOf', opts.asOf);
    for (const [k, v] of Object.entries(opts.params ?? {})) search.set(k, v);
    const qs = search.toString();
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
