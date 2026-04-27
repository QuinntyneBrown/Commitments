import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-weekly-focus-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './weekly-focus-tile.component.html',
  styleUrls: ['./weekly-focus-tile.component.scss']
})
export class WeeklyFocusTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.weekly-focus',
    displayName: 'Weekly Focus',
    description: 'Current weekly focus areas.',
    icon: 'date_range',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 3, y: 0 },
    includeByDefault: true
  };
}
