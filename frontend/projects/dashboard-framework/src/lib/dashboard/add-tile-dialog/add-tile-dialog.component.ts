import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

import { DashboardLayoutStore } from '../dashboard-layout.store';
import { TileRegistryService } from '../../tile-registration';

@Component({
  selector: 'df-add-tile-dialog',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './add-tile-dialog.component.html',
  styleUrls: ['./add-tile-dialog.component.scss']
})
export class AddTileDialogComponent {
  private readonly registry = inject(TileRegistryService);
  private readonly layoutStore = inject(DashboardLayoutStore);
  private readonly dialogRef = inject(MatDialogRef<AddTileDialogComponent>);

  protected readonly tiles = computed(() => this.registry.tilesForMode('live'));
  protected readonly selectedTileId = signal<string>('');
  protected readonly canConfirm = computed(() => this.selectedTileId() !== '');

  protected select(tileId: string): void {
    this.selectedTileId.set(tileId);
  }

  protected cancel(): void {
    this.dialogRef.close();
  }

  protected confirm(): void {
    const tileId = this.selectedTileId();

    if (tileId) {
      this.layoutStore.addTile(tileId);
      this.dialogRef.close(tileId);
    }
  }
}
