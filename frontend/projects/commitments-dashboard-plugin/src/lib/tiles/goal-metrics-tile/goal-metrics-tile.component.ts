// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, Input, computed, inject } from '@angular/core';
import {
  TILE_CONTEXT,
  TileContext,
  TileMetadata,
  bindTileMode
} from '@commitments/dashboard-framework';
import { StatusPillComponent, TileShellComponent } from '@commitments/ui';

import { GoalMetricsController } from './goal-metrics.controller';

@Component({
  selector: 'commitments-goal-metrics-tile',
  standalone: true,
  imports: [TileShellComponent, StatusPillComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [GoalMetricsController],
  templateUrl: './goal-metrics-tile.component.html',
  styleUrls: ['./goal-metrics-tile.component.scss']
})
export class GoalMetricsTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.goal-metrics',
    displayName: 'Goal Metrics',
    description: 'Progress against a single goal target. Updates live; reviewable at any prior date.',
    icon: 'track_changes',
    category: 'Commitments',
    defaultSize: { cols: 6, rows: 3 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: false,
    supportedModes: ['live', 'review']
  };

  protected readonly controller = inject(GoalMetricsController);
  protected readonly statusLabel = computed(() => this.controller.mode().toUpperCase());

  @Input() set goalId(value: string) {
    this.controller.setGoalId(value);
  }

  constructor() {
    this.controller.setGoalId('demo-goal');
    const context = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
    bindTileMode({
      context,
      load: (mode, asOf) => this.controller.load(mode, asOf)
    });
  }
}
