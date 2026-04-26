import { ChangeDetectionStrategy, Component, computed, effect, inject, signal } from '@angular/core';
import { ModeToggleComponent } from '@commitments/ui';

import { DashboardGridComponent } from './dashboard-grid.component';
import { DashboardLayoutStore } from './dashboard-layout.store';
import { DashboardModeService } from './dashboard-mode.service';
import { ReviewScrubberComponent } from './review-scrubber/review-scrubber.component';
import { DashboardMode, TileRegistryService } from '../tile-registration';

@Component({
  selector: 'commitments-dashboard-shell',
  standalone: true,
  imports: [DashboardGridComponent, ModeToggleComponent, ReviewScrubberComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard-shell.component.html',
  styleUrls: ['./dashboard-shell.component.scss']
})
export class DashboardShellComponent {
  protected readonly layoutStore = inject(DashboardLayoutStore);
  protected readonly modeService = inject(DashboardModeService);
  private readonly registry = inject(TileRegistryService);

  protected readonly tiles = computed(() =>
    this.registry.tilesForMode(this.modeService.mode())
  );
  protected readonly selectedTileId = signal('');

  constructor() {
    effect(() => {
      const available = this.tiles();
      const firstTileId = available[0]?.tileId ?? '';
      const current = this.selectedTileId();
      const stillAvailable = available.some(t => t.tileId === current);

      if (!stillAvailable && firstTileId) {
        this.selectedTileId.set(firstTileId);
      }
    });
  }

  protected addSelectedTile(): void {
    const tileId = this.selectedTileId();

    if (tileId) {
      this.layoutStore.addTile(tileId);
    }
  }

  protected onModeChange(mode: DashboardMode): void {
    this.modeService.setMode(mode);
  }
}
