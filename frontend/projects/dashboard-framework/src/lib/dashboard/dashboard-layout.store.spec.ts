import { Injector, Signal, signal } from '@angular/core';

import { TileDescriptor, TileRegistryService } from '../tile-registration';
import { DashboardLayoutStore } from './dashboard-layout.store';
import { DashboardModeService } from './dashboard-mode.service';
import { LayoutPersistenceService } from './layout-persistence.service';

class FakeTileRegistry {
  private readonly _tiles = signal<TileDescriptor[]>([]);
  readonly tiles: Signal<TileDescriptor[]> = this._tiles.asReadonly();

  setTiles(tiles: TileDescriptor[]): void {
    this._tiles.set(tiles);
  }

  getTile(tileId: string): TileDescriptor | undefined {
    return this._tiles().find(t => t.tileId === tileId);
  }

  listTiles(): TileDescriptor[] {
    return this._tiles();
  }
}

function tileDescriptor(tileId: string): TileDescriptor {
  return {
    tileId,
    displayName: tileId,
    componentType: class {} as unknown as TileDescriptor['componentType'],
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: true
  };
}

function createStore() {
  const registry = new FakeTileRegistry();
  registry.setTiles([tileDescriptor('alpha'), tileDescriptor('beta')]);

  const injector = Injector.create({
    providers: [
      DashboardLayoutStore,
      LayoutPersistenceService,
      DashboardModeService,
      { provide: TileRegistryService, useValue: registry }
    ]
  });

  const store = injector.get(DashboardLayoutStore);
  const modeService = injector.get(DashboardModeService);
  store.hydrate();
  return { store, modeService };
}

describe('DashboardLayoutStore mode awareness', () => {
  beforeEach(() => {
    localStorage.clear();
  });

  it('keeps a separate items list for each mode', () => {
    const { store, modeService } = createStore();
    expect(modeService.mode()).toBe('live');
    const liveCount = store.items().length;

    modeService.setMode('review');

    expect(store.items().length).not.toBe(liveCount + 1);
  });

  it('removing a tile in live mode does not affect the review layout', () => {
    const { store, modeService } = createStore();

    modeService.setMode('review');
    const reviewBefore = [...store.items()];

    modeService.setMode('live');
    expect(store.items().length).toBeGreaterThan(0);
    store.removeTile(store.items()[0].instanceId);

    modeService.setMode('review');
    expect(store.items()).toEqual(reviewBefore);
  });

  it('addTile only mutates the current mode layout', () => {
    const { store, modeService } = createStore();
    expect(modeService.mode()).toBe('live');
    const liveCountBefore = store.items().length;

    modeService.setMode('review');
    const reviewCountBefore = store.items().length;
    store.addTile('alpha');

    expect(store.items().length).toBe(reviewCountBefore + 1);

    modeService.setMode('live');
    expect(store.items().length).toBe(liveCountBefore);
  });
});
