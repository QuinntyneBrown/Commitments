import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-relations-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <commitments-tile-shell title="Relations" eyebrow="Balance">
      <div class="relations">
        <span>Health</span><strong>42%</strong>
        <span>Work</span><strong>33%</strong>
        <span>Personal</span><strong>25%</strong>
      </div>
    </commitments-tile-shell>
  `,
  styles: [
    `
      .relations {
        display: grid;
        grid-template-columns: 1fr auto;
        gap: 10px 16px;
        color: #5b6b84;
        font-size: 13px;
      }

      .relations strong {
        color: #172033;
      }
    `
  ]
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
