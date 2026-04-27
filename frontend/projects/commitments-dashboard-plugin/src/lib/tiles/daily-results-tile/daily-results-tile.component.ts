import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-daily-results-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './daily-results-tile.component.html',
  styleUrls: ['./daily-results-tile.component.scss']
})
export class DailyResultsTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.daily-results',
    displayName: 'Daily Results',
    description: 'Daily commitment completion summary.',
    icon: 'today',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: true
  };
}
