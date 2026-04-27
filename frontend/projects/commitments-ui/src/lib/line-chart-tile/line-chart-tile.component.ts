import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { DeltaBadgeComponent } from '../delta-badge/delta-badge.component';
import { StatusPillComponent } from '../status-pill/status-pill.component';

@Component({
  selector: 'cui-line-chart-tile',
  standalone: true,
  imports: [DeltaBadgeComponent, MatCardModule, MatIconModule, StatusPillComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './line-chart-tile.component.html',
  styleUrls: ['./line-chart-tile.component.scss']
})
export class CuiLineChartTileComponent {
  readonly title = input('Consistency Trend');
  readonly subtitle = input('7-day rolling average - last 14 days');
  readonly value = input('');
  readonly caption = input('');
  readonly delta = input(0);
  readonly deltaCaption = input('');
  readonly rangeLabel = input('');
  readonly points = input<readonly number[]>([50, 62, 54, 66, 70, 58, 74, 78, 72, 80, 83, 78, 82, 95]);
  readonly labels = input<readonly string[]>(['Apr 13', 'Apr 16', 'Apr 19', 'Apr 22', 'Apr 25']);

  protected readonly polylinePoints = computed(() => this.toPolyline(this.points()));
  protected readonly areaPath = computed(() => {
    const line = this.toPolyline(this.points());
    return line ? `M ${line} L 100 42 L 0 42 Z` : '';
  });

  private toPolyline(values: readonly number[]): string {
    if (!values.length) return '';
    const last = Math.max(1, values.length - 1);
    return values
      .map((value, index) => {
        const x = (index / last) * 100;
        const y = 42 - (Math.min(100, Math.max(0, value)) / 100) * 38;
        return `${x.toFixed(2)},${y.toFixed(2)}`;
      })
      .join(' ');
  }
}
