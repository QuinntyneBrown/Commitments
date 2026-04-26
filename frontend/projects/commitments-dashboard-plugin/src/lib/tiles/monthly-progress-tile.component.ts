import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-monthly-progress-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <commitments-tile-shell title="Monthly Progress" eyebrow="30 days">
      <div class="bars" aria-label="Monthly progress by week">
        <span style="height: 35%"></span>
        <span style="height: 72%"></span>
        <span style="height: 58%"></span>
        <span style="height: 86%"></span>
      </div>
    </commitments-tile-shell>
  `,
  styles: [
    `
      .bars {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        align-items: end;
        gap: 10px;
        height: 100%;
        min-height: 120px;
      }

      .bars span {
        display: block;
        min-height: 18px;
        border-radius: 6px 6px 0 0;
        background: linear-gradient(180deg, #2f6db3, #8bb8e8);
      }
    `
  ]
})
export class MonthlyProgressTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.monthly-progress',
    displayName: 'Monthly Progress',
    description: 'Progress trend for the current month.',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 6, y: 0 },
    includeByDefault: true
  };
}
