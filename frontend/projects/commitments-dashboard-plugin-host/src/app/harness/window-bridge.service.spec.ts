import { readFileSync } from 'fs';
import { join } from 'path';

describe('WindowBridgeService (source)', () => {
  const ts = readFileSync(join(__dirname, 'window-bridge.service.ts'), 'utf8');

  it('writes snapshot to window.__pluginHarness in constructor', () => {
    expect(ts).toMatch(/window.*__pluginHarness\s*=\s*this\.snapshot/);
  });

  it('setTile mutates snapshot.tileId', () => {
    expect(ts).toMatch(/setTile.*tileId.*this\.snapshot\.tileId\s*=/s);
  });

  it('setMode mutates snapshot.mode', () => {
    expect(ts).toMatch(/setMode.*mode.*this\.snapshot\.mode\s*=/s);
  });

  it('setAsOf mutates snapshot.asOf', () => {
    expect(ts).toMatch(/setAsOf.*asOf.*this\.snapshot\.asOf\s*=/s);
  });

  it('reset clears http and chart arrays', () => {
    expect(ts).toMatch(/snapshot\.http\.length\s*=\s*0/);
    expect(ts).toMatch(/snapshot\.chart\.length\s*=\s*0/);
  });

  it('is provided in root', () => {
    expect(ts).toMatch(/providedIn:\s*['"]root['"]/);
  });
});

describe('TileHarnessComponent bridge integration (source)', () => {
  const ts = readFileSync(join(__dirname, 'tile-harness.component.ts'), 'utf8');

  it('injects WindowBridgeService', () => {
    expect(ts).toMatch(/WindowBridgeService/);
  });

  it('calls bridge.reset() on each route change', () => {
    expect(ts).toMatch(/bridge\.reset\(\)/);
  });

  it('calls bridge.setTile with the tileId', () => {
    expect(ts).toMatch(/bridge\.setTile\(/);
  });

  it('calls bridge.setMode and bridge.setAsOf on query param change', () => {
    expect(ts).toMatch(/bridge\.setMode\(/);
    expect(ts).toMatch(/bridge\.setAsOf\(/);
  });
});
