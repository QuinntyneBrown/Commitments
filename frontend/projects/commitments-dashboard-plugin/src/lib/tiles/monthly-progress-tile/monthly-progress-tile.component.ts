// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  TILE_CONTEXT,
  TileMetadata,
  bindTileMode
} from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

import { MonthlyProgressController } from './monthly-progress.controller';

@Component({
  selector: 'commitments-monthly-progress-tile',
  standalone: true,
  imports: [TileShellComponent],
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

  constructor() {
    const context = inject(TILE_CONTEXT, { optional: true });
    bindTileMode({
      context,
      load: (mode, asOf) => this.controller.load(mode, asOf)
    });
  }
}
