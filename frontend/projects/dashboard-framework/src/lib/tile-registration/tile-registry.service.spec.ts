import { TileDescriptor } from './tile.model';
import { TileRegistryService } from './tile-registry.service';

function descriptor(tileId: string, supportedModes?: ReadonlyArray<'live' | 'review'>): TileDescriptor {
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

describe('TileRegistryService.tilesForMode', () => {
  let registry: TileRegistryService;

  beforeEach(() => {
    registry = new TileRegistryService();
  });

  it('returns every tile when none declare supportedModes (back-compat)', () => {
    registry.registerTile(descriptor('alpha'));
    registry.registerTile(descriptor('beta'));

    expect(registry.tilesForMode('live').map(t => t.tileId)).toEqual(['alpha', 'beta']);
    expect(registry.tilesForMode('review').map(t => t.tileId)).toEqual(['alpha', 'beta']);
  });

  it('filters by supportedModes when specified', () => {
    registry.registerTile(descriptor('live-only', ['live']));
    registry.registerTile(descriptor('review-only', ['review']));
    registry.registerTile(descriptor('shared', ['live', 'review']));

    expect(registry.tilesForMode('live').map(t => t.tileId)).toEqual(['live-only', 'shared']);
    expect(registry.tilesForMode('review').map(t => t.tileId)).toEqual(['review-only', 'shared']);
  });
});
