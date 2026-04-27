import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-outstanding-todos-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './outstanding-todos-tile.component.html',
  styleUrls: ['./outstanding-todos-tile.component.scss']
})
export class OutstandingTodosTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.outstanding-todos',
    displayName: 'Outstanding To‑Dos',
    description: 'Open tasks that still need action.',
    icon: 'checklist',
    category: 'Tasks',
    defaultSize: { cols: 3, rows: 3 },
    defaultPosition: { x: 9, y: 0 },
    includeByDefault: true
  };
}
