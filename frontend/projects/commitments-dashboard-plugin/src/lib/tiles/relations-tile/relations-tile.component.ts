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

import { RelationsController } from './relations.controller';

@Component({
  selector: 'commitments-relations-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [RelationsController],
  templateUrl: './relations-tile.component.html',
  styleUrls: ['./relations-tile.component.scss']
})
export class RelationsTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.relations',
    displayName: 'Relations',
    description: 'Commitment distribution by relation.',
    icon: 'diversity_3',
    category: 'Commitments',
    defaultSize: { cols: 4, rows: 2 },
    defaultPosition: { x: 0, y: 2 },
    includeByDefault: true,
    supportedModes: ['live', 'review']
  };

  protected readonly controller = inject(RelationsController);

  constructor() {
    const context = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
    bindTileMode({
      context,
      load: (mode, asOf) => this.controller.load(mode, asOf)
    });
  }
}
