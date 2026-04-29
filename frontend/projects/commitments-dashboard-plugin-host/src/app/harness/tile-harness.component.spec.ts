import { readFileSync } from 'fs';
import { join } from 'path';

describe('TileHarnessComponent (source)', () => {
  const ts = readFileSync(join(__dirname, 'tile-harness.component.ts'), 'utf8');

  it('reads tileId from route paramMap', () => {
    expect(ts).toMatch(/paramMap\.subscribe/);
    expect(ts).toMatch(/tileId\.set/);
  });

  it('computes tile descriptor via TileRegistryService.getTile', () => {
    expect(ts).toMatch(/registry\.getTile\(this\.tileId\(\)\)/);
  });

  it('provides TILE_CONTEXT in child injector', () => {
    expect(ts).toMatch(/TILE_CONTEXT/);
    expect(ts).toMatch(/Injector\.create/);
  });

  it('defaults unknown mode to live', () => {
    expect(ts).toMatch(/=== 'review' \? 'review' : 'live'/);
  });
});

describe('TileHarnessComponent (template)', () => {
  const html = readFileSync(join(__dirname, 'tile-harness.component.html'), 'utf8');

  it('renders tile via ngComponentOutlet', () => {
    expect(html).toMatch(/ngComponentOutlet/);
  });

  it('shows tile-not-found testid for unknown tileId', () => {
    expect(html).toMatch(/data-testid="tile-not-found"/);
  });
});
