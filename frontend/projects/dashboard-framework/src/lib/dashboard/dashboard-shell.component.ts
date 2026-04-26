import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { ModeToggleComponent } from '@commitments/ui';

import { DashboardGridComponent } from './dashboard-grid.component';
import { DashboardLayoutStore } from './dashboard-layout.store';
import { DashboardModeService } from './dashboard-mode.service';
import { DashboardMode, TileRegistryService } from '../tile-registration';

@Component({
  selector: 'commitments-dashboard-shell',
  standalone: true,
  imports: [DashboardGridComponent, ModeToggleComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard-shell.component.html',
  styleUrls: ['./dashboard-shell.component.scss']
})
export class DashboardShellComponent {
  protected readonly layoutStore = inject(DashboardLayoutStore);
  protected readonly modeService = inject(DashboardModeService);
  private readonly registry = inject(TileRegistryService);

  protected readonly tiles = this.registry.tiles;
  protected readonly selectedTileId = signal('');

  constructor() {
    effect(() => {
      const firstTileId = this.tiles()[0]?.tileId ?? '';

      if (!this.selectedTileId() && firstTileId) {
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
