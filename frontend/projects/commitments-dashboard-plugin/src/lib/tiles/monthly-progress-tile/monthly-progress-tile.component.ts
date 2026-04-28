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

import { MonthlyProgressController } from './monthly-progress.controller';

@Component({
  selector: 'commitments-monthly-progress-tile',
  standalone: true,
  imports: [TileShellComponent, StatusPillComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MonthlyProgressController],
  templateUrl: './monthly-progress-tile.component.html',
  styleUrls: ['./monthly-progress-tile.component.scss']
})
export class MonthlyProgressTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.monthly-progress',
    displayName: 'Monthly Progress',
    description: 'Progress trend for the current month.',
    icon: 'calendar_month',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 6, y: 0 },
    includeByDefault: true,
    supportedModes: ['live', 'review']
  };

  protected readonly controller = inject(MonthlyProgressController);
  protected readonly statusLabel = computed(() => this.controller.mode().toUpperCase());

  constructor() {
    const context = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
    bindTileMode({
      context,
      load: (mode, asOf) => this.controller.load(mode, asOf)
    });
  }
}
