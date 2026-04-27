import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { DeltaBadgeComponent } from '../delta-badge/delta-badge.component';
import { StatusPillComponent } from '../status-pill/status-pill.component';

@Component({
  selector: 'cui-real-time-metric-tile',
  standalone: true,
  imports: [DeltaBadgeComponent, MatCardModule, MatIconModule, StatusPillComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './real-time-metric-tile.component.html',
  styleUrls: ['./real-time-metric-tile.component.scss']
})
export class CuiRealTimeMetricTileComponent {
  readonly title = input('Commitment Performance');
  readonly subtitle = input('% completed - last 14 days');
  readonly value = input('');
  readonly caption = input('');
  readonly delta = input(0);
  readonly deltaCaption = input('');
  readonly bars = input<readonly number[]>([70, 60, 80, 90, 50, 70, 85, 75, 80, 95, 88, 92, 78, 82]);
  readonly labels = input<readonly string[]>(['M', 'T', 'W', 'T', 'F', 'S', 'S', 'M', 'T', 'W', 'T', 'F', 'S', 'Today']);

  protected barHeight(value: number): string {
    return `${Math.min(100, Math.max(4, value))}%`;
  }
}
