import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { DashboardGridComponent } from './dashboard-grid.component';
import { DashboardLayoutStore } from './dashboard-layout.store';
import { TileRegistryService } from '../tile-registration';

@Component({
  selector: 'commitments-dashboard-shell',
  standalone: true,
  imports: [DashboardGridComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard-shell.component.html',
  styleUrls: ['./dashboard-shell.component.scss']
})
export class DashboardShellComponent {
  protected readonly layoutStore = inject(DashboardLayoutStore);
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
}
