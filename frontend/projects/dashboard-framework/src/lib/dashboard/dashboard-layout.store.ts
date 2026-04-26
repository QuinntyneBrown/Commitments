import { Injectable, Signal, computed, inject, signal } from '@angular/core';
import { TileDescriptor, TileRegistryService } from '../tile-registration';
import {
  DEFAULT_COLS,
  DEFAULT_ROWS,
  DashboardItem,
  GridsterConfig,
  PersistedLayout
} from './dashboard.model';
import { LayoutPersistenceService } from './layout-persistence.service';

const FALLBACK_SIZE = { cols: 3, rows: 2 };

@Injectable({ providedIn: 'root' })
export class DashboardLayoutStore {
  private readonly registry = inject(TileRegistryService);
  private readonly persistence = inject(LayoutPersistenceService);
  private readonly itemsSignal = signal<DashboardItem[]>([]);
  private readonly editModeSignal = signal(false);

  readonly items: Signal<DashboardItem[]> = this.itemsSignal.asReadonly();
  readonly isEditMode: Signal<boolean> = this.editModeSignal.asReadonly();
  readonly gridOptions: Signal<GridsterConfig> = computed(() => this.buildGridOptions(this.editModeSignal()));

  hydrate(): void {
    const persisted = this.persistence.load();
    const persistedItems = persisted ? this.filterKnownTiles(persisted.items) : [];

    this.itemsSignal.set(persistedItems.length > 0 ? persistedItems : this.defaultItems());
  }

  toggleEditMode(): void {
    this.editModeSignal.update((value) => !value);
  }

  setEditMode(value: boolean): void {
    this.editModeSignal.set(value);
  }

  addTile(tileId: string): void {
    const descriptor = this.registry.getTile(tileId);

    if (!descriptor) {
      console.warn(`[dashboard-framework] No tile registered for "${tileId}".`);
      return;
    }

    this.itemsSignal.update((items) => [...items, this.createItem(descriptor, items.length)]);
    this.persist();
  }

  removeTile(instanceId: string): void {
    this.itemsSignal.update((items) => items.filter((item) => item.instanceId !== instanceId));
    this.persist();
  }

  updateItem(instanceId: string, patch: Partial<DashboardItem>): void {
    this.itemsSignal.update((items) =>
      items.map((item) => (item.instanceId === instanceId ? { ...item, ...patch } : item))
    );
    this.persist();
  }

  resetLayout(): void {
    this.persistence.clear();
    this.itemsSignal.set(this.defaultItems());
  }

  private filterKnownTiles(items: readonly DashboardItem[]): DashboardItem[] {
    return items.filter((item) => this.registry.getTile(item.tileId));
  }

  private defaultItems(): DashboardItem[] {
    return this.registry
      .listTiles()
      .filter((descriptor) => descriptor.includeByDefault)
      .map((descriptor, index) => this.createItem(descriptor, index));
  }

  private createItem(descriptor: TileDescriptor, index: number): DashboardItem {
    const size = descriptor.defaultSize ?? FALLBACK_SIZE;
    const position = descriptor.defaultPosition ?? {
      x: (index * size.cols) % DEFAULT_COLS,
      y: Math.floor((index * size.cols) / DEFAULT_COLS) * size.rows
    };

    return {
      instanceId: this.newInstanceId(descriptor.tileId),
      tileId: descriptor.tileId,
      cols: size.cols,
      rows: size.rows,
      x: position.x,
      y: position.y,
      maximized: false
    };
  }

  private persist(): void {
    const layout: PersistedLayout = {
      schemaVersion: 1,
      savedAt: Date.now(),
      items: this.itemsSignal()
    };
    this.persistence.save(layout);
  }

  private buildGridOptions(editMode: boolean): GridsterConfig {
    return {
      draggable: { enabled: editMode },
      resizable: { enabled: editMode },
      pushItems: editMode,
      swap: editMode,
      displayGrid: editMode ? 'always' : 'none',
      gridType: 'fit',
      margin: 16,
      outerMargin: true,
      minCols: DEFAULT_COLS,
      maxCols: DEFAULT_COLS,
      minRows: DEFAULT_ROWS,
      maxRows: 24
    };
  }

  private newInstanceId(tileId: string): string {
    return `${tileId}-${Date.now().toString(36)}-${Math.random().toString(36).slice(2, 8)}`;
  }
}
