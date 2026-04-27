import { effect } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

import { DashboardMode, TileContext } from './tile.model';

export interface BindTileModeOptions {
  readonly context: TileContext | null;
  readonly invalidations$?: Observable<unknown>;
  readonly load: (mode: DashboardMode, asOf: string | null) => void;
}

export function bindTileMode(opts: BindTileModeOptions): void {
  const { context, invalidations$, load } = opts;

  if (!context) {
    load('live', null);
    return;
  }

  const read = (): [DashboardMode, string | null] => [context.mode(), context.selectedReviewDate()];

  effect(() => load(...read()));
  context.refresh$.pipe(takeUntilDestroyed()).subscribe(() => load(...read()));
  invalidations$?.pipe(takeUntilDestroyed()).subscribe(() => load(...read()));
}
