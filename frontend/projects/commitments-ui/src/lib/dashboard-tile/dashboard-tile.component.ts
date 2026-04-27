import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

export type CuiTileAccent = 'live' | 'review' | 'chart' | 'success';

@Component({
  selector: 'cui-dashboard-tile',
  standalone: true,
  imports: [MatCardModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard-tile.component.html',
  styleUrls: ['./dashboard-tile.component.scss']
})
export class CuiDashboardTileComponent {
  readonly icon = input('today');
  readonly title = input('Daily Results');
  readonly value = input('');
  readonly caption = input('');
  readonly accent = input<CuiTileAccent>('live');
}
