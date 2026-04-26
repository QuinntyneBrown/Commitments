import { Injectable, Signal, computed, inject, signal } from '@angular/core';

import { DashboardMode, TileDescriptor, TileRegistryService } from '../tile-registration';
import {
  DEFAULT_COLS,
  DEFAULT_ROWS,
  DashboardItem,
  GridsterConfig,
  PersistedLayout
} from './dashboard.model';
import { DashboardModeService } from './dashboard-mode.service';
import { LayoutPersistenceService } from './layout-persistence.service';

const FALLBACK_SIZE = { cols: 3, rows: 2 };

@Injectable({ providedIn: 'root' })
export class DashboardLayoutStore {
  private readonly registry = inject(TileRegistryService);
  private readonly persistence = inject(LayoutPersistenceService);
  private readonly modeService = inject(DashboardModeService);

  private readonly liveItems = signal<DashboardItem[]>([]);
  private readonly reviewItems = signal<DashboardItem[]>([]);
  private readonly editModeSignal = signal(false);

  readonly liveLayout: Signal<DashboardItem[]> = this.liveItems.asReadonly();
  readonly reviewLayout: Signal<DashboardItem[]> = this.reviewItems.asReadonly();
  readonly items: Signal<DashboardItem[]> = computed(() =>
    this.modeService.mode() === 'live' ? this.liveItems() : this.reviewItems()
  );
  readonly isEditMode: Signal<boolean> = this.editModeSignal.asReadonly();
  readonly gridOptions: Signal<GridsterConfig> = computed(() => this.buildGridOptions(this.editModeSignal()));

  hydrate(): void {
    this.liveItems.set(this.loadOrSeed('live'));
    this.reviewItems.set(this.loadOrSeed('review'));
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

    this.mutateCurrent((items) => [...items, this.createItem(descriptor, items.length)]);
  }

  removeTile(instanceId: string): void {
    this.mutateCurrent((items) => items.filter((item) => item.instanceId !== instanceId));
  }

  updateItem(instanceId: string, patch: Partial<DashboardItem>): void {
    this.mutateCurrent((items) =>
      items.map((item) => (item.instanceId === instanceId ? { ...item, ...patch } : item))
    );
  }

  resetLayout(): void {
    const mode = this.modeService.mode();
    this.persistence.clear(mode);
    this.signalForMode(mode).set(this.defaultItems());
  }

  private signalForMode(mode: DashboardMode) {
    return mode === 'live' ? this.liveItems : this.reviewItems;
  }

  private mutateCurrent(transform: (items: DashboardItem[]) => DashboardItem[]): void {
    const mode = this.modeService.mode();
    const target = this.signalForMode(mode);
    target.update(transform);
    this.persist(mode, target());
  }

  private loadOrSeed(mode: DashboardMode): DashboardItem[] {
    const persisted = this.persistence.load(mode);
    const persistedItems = persisted ? this.filterKnownTiles(persisted.items) : [];
    return persistedItems.length > 0 ? persistedItems : this.defaultItems();
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

  private persist(mode: DashboardMode, items: DashboardItem[]): void {
    const layout: PersistedLayout = {
      schemaVersion: 1,
      savedAt: Date.now(),
      items
    };
    this.persistence.save(mode, layout);
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
