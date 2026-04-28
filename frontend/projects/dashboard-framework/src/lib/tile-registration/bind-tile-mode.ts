import { effect } from '@angular/core';

import { DashboardMode, TileContext } from './tile.model';

export interface BindTileModeOptions {
  readonly context: TileContext | null;
  readonly load: (mode: DashboardMode, asOf: string | null) => void;
}

export function bindTileMode(opts: BindTileModeOptions): void {
  const { context, load } = opts;

  if (!context) {
    load('live', null);
    return;
  }

  effect(() => load(context.mode(), context.selectedReviewDate()));
}
