// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import {
  TILE_CONTEXT,
  TileContext,
  TileMetadata,
  bindTileMode
} from '@commitments/dashboard-framework';
import { StatusPillComponent, TileShellComponent } from '@commitments/ui';

import { DailyResultsController } from './daily-results.controller';

@Component({
  selector: 'commitments-daily-results-tile',
  standalone: true,
  imports: [TileShellComponent, StatusPillComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DailyResultsController],
  templateUrl: './daily-results-tile.component.html',
  styleUrls: ['./daily-results-tile.component.scss']
})
export class DailyResultsTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.daily-results',
    displayName: 'Daily Results',
    description: 'Daily commitment completion summary.',
    icon: 'today',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: true,
    supportedModes: ['live', 'review']
  };

  protected readonly controller = inject(DailyResultsController);
  protected readonly statusLabel = computed(() => this.controller.mode().toUpperCase());

  constructor() {
    const context = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
    bindTileMode({
      context,
      load: (mode, asOf) => this.controller.load(mode, asOf)
    });
  }
}
