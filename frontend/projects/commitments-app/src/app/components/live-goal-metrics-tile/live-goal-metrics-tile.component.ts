// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

import { LiveGoalMetricsController } from './live-goal-metrics-controller';

@Component({
  selector: 'app-live-goal-metrics-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [LiveGoalMetricsController],
  templateUrl: './live-goal-metrics-tile.component.html',
  styleUrls: ['./live-goal-metrics-tile.component.scss']
})
export class LiveGoalMetricsTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.live-goal-metrics',
    displayName: 'Live Goal Metrics',
    description: 'Real-time progress against a single goal target.',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: false
  };

  constructor(readonly controller: LiveGoalMetricsController) {}
}
