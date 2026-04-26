import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TileMetadata } from '@commitments/dashboard-framework';
import { TileShellComponent } from '@commitments/ui';

@Component({
  selector: 'commitments-daily-results-tile',
  standalone: true,
  imports: [TileShellComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <commitments-tile-shell title="Daily Results" eyebrow="Today" status="Live">
      <div class="metric">
        <span class="metric__value">7 / 9</span>
        <span class="metric__label">commitments completed</span>
      </div>
      <div class="progress" aria-label="Daily progress">
        <span style="width: 78%"></span>
      </div>
    </commitments-tile-shell>
  `,
  styles: [
    `
      .metric {
        display: grid;
        gap: 4px;
      }

      .metric__value {
        color: #1d5f3f;
        font-size: 42px;
        font-weight: 800;
        line-height: 1;
      }

      .metric__label {
        color: #5b6b84;
        font-size: 13px;
      }

      .progress {
        overflow: hidden;
        height: 8px;
        margin-top: 18px;
        border-radius: 999px;
        background: #e6edf4;
      }

      .progress span {
        display: block;
        height: 100%;
        border-radius: inherit;
        background: #2e9d68;
      }
    `
  ]
})
export class DailyResultsTileComponent {
  static readonly tileMetadata: TileMetadata = {
    tileId: 'commitments.daily-results',
    displayName: 'Daily Results',
    description: 'Daily commitment completion summary.',
    category: 'Commitments',
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: true
  };
}
