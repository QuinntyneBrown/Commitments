import { MisconfiguredTileError } from './misconfigured-tile-error';
import { TileDescriptor } from './tile.model';
import { TileRegistryService } from './tile-registry.service';

function descriptor(
  tileId: string,
  supportedModes?: ReadonlyArray<'live' | 'review'>
): TileDescriptor {
  return {
    tileId,
    displayName: tileId,
    componentType: class {} as unknown as TileDescriptor['componentType'],
    defaultSize: { cols: 3, rows: 2 },
    defaultPosition: { x: 0, y: 0 },
    includeByDefault: false,
    supportedModes
  };
}

describe('TileRegistryService.registerTile (mode-invariant validation)', () => {
  let registry: TileRegistryService;

  beforeEach(() => {
    registry = new TileRegistryService();
  });

  it('accepts a descriptor that omits supportedModes', () => {
    expect(() => registry.registerTile(descriptor('alpha'))).not.toThrow();
    expect(registry.getTile('alpha')).toBeDefined();
  });

  it("accepts a descriptor with supportedModes ['live','review']", () => {
    expect(() => registry.registerTile(descriptor('alpha', ['live', 'review']))).not.toThrow();
  });

  it("accepts a descriptor with supportedModes ['review','live'] (any order)", () => {
    expect(() => registry.registerTile(descriptor('alpha', ['review', 'live']))).not.toThrow();
  });

  it("throws MisconfiguredTileError when supportedModes is ['live']", () => {
    expect(() => registry.registerTile(descriptor('live-only', ['live']))).toThrow(
      MisconfiguredTileError
    );
  });

  it("throws MisconfiguredTileError when supportedModes is ['review']", () => {
    expect(() => registry.registerTile(descriptor('review-only', ['review']))).toThrow(
      MisconfiguredTileError
    );
  });

  it('throws MisconfiguredTileError when supportedModes is empty', () => {
    expect(() => registry.registerTile(descriptor('empty', []))).toThrow(MisconfiguredTileError);
  });

  it("throws MisconfiguredTileError when supportedModes is ['live','live']", () => {
    expect(() => registry.registerTile(descriptor('dup', ['live', 'live']))).toThrow(
      MisconfiguredTileError
    );
  });

  it('error message names the offending tileId', () => {
    expect(() => registry.registerTile(descriptor('offender', ['live']))).toThrow(/offender/);
  });

  it('does not register the tile when validation fails', () => {
    try {
      registry.registerTile(descriptor('offender', ['live']));
    } catch {
      // ignore
    }
    expect(registry.getTile('offender')).toBeUndefined();
    expect(registry.listTiles()).toHaveLength(0);
  });
});

describe('TileRegistryService.tilesForMode (defensive identity)', () => {
  let registry: TileRegistryService;

  beforeEach(() => {
    registry = new TileRegistryService();
  });

  it('returns identical descriptor lists for live and review', () => {
    registry.registerTile(descriptor('alpha'));
    registry.registerTile(descriptor('beta', ['live', 'review']));
    registry.registerTile(descriptor('gamma', ['review', 'live']));

    const live = registry.tilesForMode('live').map((t) => t.tileId);
    const review = registry.tilesForMode('review').map((t) => t.tileId);
    expect(live).toEqual(['alpha', 'beta', 'gamma']);
    expect(review).toEqual(live);
  });
});
