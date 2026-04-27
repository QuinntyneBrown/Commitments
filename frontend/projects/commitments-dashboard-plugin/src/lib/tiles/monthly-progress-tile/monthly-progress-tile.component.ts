import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-monthly-progress-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './monthly-progress-tile.component.html',
  styleUrls: ['./monthly-progress-tile.component.scss']
})
export class MonthlyProgressTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.monthly-progress',
    displayName: 'Monthly Progress',
    description: 'Progress trend for the current month.',
    icon: 'calendar_month',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 6, y: 0 },
    includeByDefault: true
  };
}
