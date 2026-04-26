// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

import { ReviewGoalHistoryController } from './review-goal-history-controller';

@Component({
  selector: 'app-review-goal-history-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [ReviewGoalHistoryController],
  templateUrl: './review-goal-history-tile.component.html',
  styleUrls: ['./review-goal-history-tile.component.scss']
})
export class ReviewGoalHistoryTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.review-goal-history',
    displayName: 'Review Goal History',
    description: 'Scrub through historical progress for a single goal.',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 3 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: false
  };

  constructor(readonly controller: ReviewGoalHistoryController) {}

  onStartChange(event: Event): void {
    this.controller.setDraftStart((event.target as HTMLInputElement).value);
  }

  onEndChange(event: Event): void {
    this.controller.setDraftEnd((event.target as HTMLInputElement).value);
  }

  onScrubChange(event: Event): void {
    this.controller.scrub(Number((event.target as HTMLInputElement).value));
  }
}
