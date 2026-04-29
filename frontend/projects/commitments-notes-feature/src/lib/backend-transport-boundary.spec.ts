import { readFileSync, readdirSync, statSync } from 'fs';
import { join } from 'path';

const libRoot = join(__dirname, '.');

function productionSources(root: string): string[] {
  return readdirSync(root).flatMap(name => {
    const path = join(root, name);
    if (statSync(path).isDirectory()) return productionSources(path);
    return path.endsWith('.ts') && !path.endsWith('.spec.ts') ? [path] : [];
  });
}

describe('commitments-notes-feature backend transport boundary', () => {
  const libSource = productionSources(libRoot).map(p => readFileSync(p, 'utf8')).join('\n');

  it('routes all HTTP through DashboardBackendService — no direct HTTP imports', () => {
    expect(libSource).not.toContain('@angular/common/http');
    expect(libSource).not.toContain('@microsoft/signalr');
    expect(libSource).not.toMatch(/\bfetch\s*\(/);
    expect(libSource).not.toMatch(/\bXMLHttpRequest\b/);
  });

  it('data services delegate to DashboardBackendService', () => {
    const serviceFiles = productionSources(join(libRoot, 'data'))
      .filter(p => p.endsWith('.service.ts'))
      .map(p => readFileSync(p, 'utf8'))
      .join('\n');
    expect(serviceFiles).toContain('DashboardBackendService');
    expect(serviceFiles).not.toMatch(/\bHttpClient\b/);
  });
});
