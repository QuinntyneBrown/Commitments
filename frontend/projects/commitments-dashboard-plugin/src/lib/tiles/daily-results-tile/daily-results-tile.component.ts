import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { TILE_CONTEXT, TileContext, TileMetadata } from '@commitments/dashboard-framework';
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

  private readonly _tileContext = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
  private readonly _fallbackMode = signal<'live' | 'review'>('live').asReadonly();

  protected readonly status = computed(() =>
    (this._tileContext?.mode ?? this._fallbackMode)() === 'review' ? 'Review' : 'Live'
  );
}
