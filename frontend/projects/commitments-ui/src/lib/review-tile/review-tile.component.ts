import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { DeltaBadgeComponent } from '../delta-badge/delta-badge.component';
import { StatusPillComponent } from '../status-pill/status-pill.component';

@Component({
  selector: 'cui-review-tile',
  standalone: true,
  imports: [DeltaBadgeComponent, MatCardModule, MatIconModule, StatusPillComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './review-tile.component.html',
  styleUrls: ['./review-tile.component.scss']
})
export class CuiReviewTileComponent {
  readonly title = input('Daily Results');
  readonly dateLabel = input('');
  readonly value = input('');
  readonly caption = input('');
  readonly delta = input(0);
  readonly deltaCaption = input('');
  readonly snapshotLabel = input('snapshot');
}
