import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-weekly-focus-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <commitments-tile-shell title="Weekly Focus" eyebrow="This week">
      <ul class="focus-list">
        <li><strong>Move</strong><span>5 sessions planned</span></li>
        <li><strong>Read</strong><span>3 sessions planned</span></li>
        <li><strong>Reflect</strong><span>2 notes pending</span></li>
      </ul>
    </commitments-tile-shell>
  `,
  styles: [
    `
      .focus-list {
        display: grid;
        gap: 10px;
        margin: 0;
        padding: 0;
        list-style: none;
      }

      .focus-list li {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 10px;
        padding: 10px 0;
        border-bottom: 1px solid #edf1f6;
      }

      .focus-list strong {
        color: #172033;
      }

      .focus-list span {
        color: #64748b;
        font-size: 13px;
      }
    `
  ]
})
export class WeeklyFocusTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.weekly-focus',
    displayName: 'Weekly Focus',
    description: 'Current weekly focus areas.',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 3, y: 0 },
    includeByDefault: true
  };
}
