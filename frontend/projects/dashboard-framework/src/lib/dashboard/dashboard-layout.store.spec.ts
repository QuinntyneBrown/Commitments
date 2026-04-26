import { Signal, signal } from '@angular/core';

import { TestBed } from '@angular/core/testing';

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

describe('DashboardLayoutStore mode awareness', () => {
  let registry: FakeTileRegistry;
  let modeService: DashboardModeService;
  let store: DashboardLayoutStore;

  beforeEach(() => {
    localStorage.clear();
    registry = new FakeTileRegistry();
    registry.setTiles([tileDescriptor('alpha'), tileDescriptor('beta')]);

    TestBed.configureTestingModule({
      providers: [
        DashboardLayoutStore,
        LayoutPersistenceService,
        DashboardModeService,
        { provide: TileRegistryService, useValue: registry }
      ]
    });

    modeService = TestBed.inject(DashboardModeService);
    store = TestBed.inject(DashboardLayoutStore);
    store.hydrate();
  });

  it('keeps a separate items list for each mode', () => {
    expect(modeService.mode()).toBe('live');
    const liveCount = store.items().length;

    modeService.setMode('review');

    expect(store.items().length).not.toBe(liveCount + 1);
  });

  it('removing a tile in live mode does not affect the review layout', () => {
    expect(modeService.mode()).toBe('live');
    const initialLive = store.items();
    expect(initialLive.length).toBeGreaterThan(0);

    modeService.setMode('review');
    const initialReview = [...store.items()];

    modeService.setMode('live');
    store.removeTile(store.items()[0].instanceId);

    modeService.setMode('review');
    expect(store.items()).toEqual(initialReview);
  });

  it('addTile only mutates the current mode layout', () => {
    expect(modeService.mode()).toBe('live');
    const liveCountBefore = store.items().length;

    modeService.setMode('review');
    const reviewCountBefore = store.items().length;
    store.addTile('alpha');
    const reviewCountAfter = store.items().length;

    expect(reviewCountAfter).toBe(reviewCountBefore + 1);

    modeService.setMode('live');
    expect(store.items().length).toBe(liveCountBefore);
  });
});
