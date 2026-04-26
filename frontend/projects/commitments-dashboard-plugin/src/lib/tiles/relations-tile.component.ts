import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-relations-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './relations-tile.component.html',
  styleUrls: ['./relations-tile.component.scss']
})
export class RelationsTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.relations',
    displayName: 'Relations',
    description: 'Commitment distribution by relation.',
    category: 'Commitments',
    defaultSize: { cols: 4, rows: 2 },
    defaultPosition: { x: 0, y: 2 },
    includeByDefault: true
  };
}
