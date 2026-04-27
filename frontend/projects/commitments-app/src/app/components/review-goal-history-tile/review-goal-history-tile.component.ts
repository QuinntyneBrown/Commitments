// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
  effect,
  inject,
  signal
} from '@angular/core';
import { TILE_CONTEXT, TileContext, TileMetadata } from '@commitments/dashboard-framework';
import {
  DeltaBadgeComponent,
  MetricHeaderComponent,
  StatusPillComponent,
  TileShellComponent
} from '@commitments/ui';

import { GoalProgressService } from '../../services/goal-progress.service';
import { ReviewGoalHistoryController } from './review-goal-history-controller';

@Component({
  selector: 'app-review-goal-history-tile',
  standalone: true,
  imports: [TileShellComponent, StatusPillComponent, MetricHeaderComponent, DeltaBadgeComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './review-goal-history-tile.component.html',
  styleUrls: ['./review-goal-history-tile.component.scss']
})
export class ReviewGoalHistoryTileComponent implements OnInit {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.review-goal-history',
    displayName: 'Review Goal History',
    description: 'Snapshot of a goal at the date selected by the dashboard scrubber.',
    icon: 'history',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 3 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: false,
    supportedModes: ['review']
  };

  @Input() goalId = '';

  private readonly _service = inject(GoalProgressService);
  private readonly _tileContext = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
  private readonly _fallbackDate = signal<string | null>(null).asReadonly();

  readonly controller = new ReviewGoalHistoryController(
    this._service,
    this._tileContext?.selectedReviewDate ?? this._fallbackDate
  );

  constructor() {
    effect(() => {
      this.controller.selectedDate();
      this.controller.refresh();
    });
  }

  ngOnInit(): void {
    if (this.goalId) {
      this.controller.load(this.goalId);
    }
  }
}
