// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  TILE_CONTEXT,
  TileContext,
  TileMetadata,
  bindTileMode
} from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

import { WeeklyFocusController } from './weekly-focus.controller';

@Component({
  selector: 'commitments-weekly-focus-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [WeeklyFocusController],
  templateUrl: './weekly-focus-tile.component.html',
  styleUrls: ['./weekly-focus-tile.component.scss']
})
export class WeeklyFocusTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.weekly-focus',
    displayName: 'Weekly Focus',
    description: 'Current weekly focus areas.',
    icon: 'date_range',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 3, y: 0 },
    includeByDefault: true,
    supportedModes: ['live', 'review']
  };

  protected readonly controller = inject(WeeklyFocusController);

  constructor() {
    const context = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
    bindTileMode({
      context,
      load: (mode, asOf) => this.controller.load(mode, asOf)
    });
  }
}
