// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import {
  TILE_CONTEXT,
  TileMetadata,
  bindTileMode
} from '@commitments/dashboard-framework';
import { StatusPillComponent, TileShellComponent } from '@commitments/ui';

import { OutstandingTodosController } from './outstanding-todos.controller';

@Component({
  selector: 'commitments-outstanding-todos-tile',
  standalone: true,
  imports: [TileShellComponent, StatusPillComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [OutstandingTodosController],
  templateUrl: './outstanding-todos-tile.component.html',
  styleUrls: ['./outstanding-todos-tile.component.scss']
})
export class OutstandingTodosTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.outstanding-todos',
    displayName: 'Outstanding Todos',
    description: 'Open tasks that still need action.',
    icon: 'checklist',
    category: 'Tasks',
    defaultSize: { cols: 3, rows: 3 },
    defaultPosition: { x: 9, y: 0 },
    includeByDefault: true,
    supportedModes: ['live', 'review']
  };

  protected readonly controller = inject(OutstandingTodosController);
  protected readonly statusLabel = computed(() => `${this.controller.count()} OPEN`);
  protected readonly pillVariant = computed<'warning' | 'review'>(() =>
    this.controller.mode() === 'review' ? 'review' : 'warning'
  );

  constructor() {
    const context = inject(TILE_CONTEXT, { optional: true });
    bindTileMode({
      context,
      load: (mode, asOf) => this.controller.load(mode, asOf)
    });
  }
}
