import { readFileSync, readdirSync, statSync } from 'fs';
import { join } from 'path';

const pluginLibRoot = join(__dirname, '..');
const dataRoot = __dirname;

function productionSources(root: string): string[] {
  return readdirSync(root)
    .flatMap((name) => {
      const path = join(root, name);
      if (statSync(path).isDirectory()) {
        return productionSources(path);
      }
      return path.endsWith('.ts') && !path.endsWith('.spec.ts') ? [path] : [];
    });
}

describe('commitments-dashboard-plugin backend transport boundary', () => {
  const pluginSource = productionSources(pluginLibRoot)
    .map((path) => readFileSync(path, 'utf8'))
    .join('\n');

  it('routes backend HTTP/WS communication through dashboard-framework services', () => {
    expect(pluginSource).not.toContain('@angular/common/http');
    expect(pluginSource).not.toContain('@microsoft/signalr');
    expect(pluginSource).not.toMatch(/\bfetch\s*\(/);
    expect(pluginSource).not.toMatch(/\bXMLHttpRequest\b/);
    expect(pluginSource).not.toMatch(/\bWebSocket\b/);
    expect(pluginSource).not.toMatch(/\bEventSource\b/);
    expect(pluginSource).not.toMatch(/\bHubConnectionBuilder\b/);
  });

  it('keeps plugin data services as delegates to DashboardBackendService', () => {
    const serviceSource = productionSources(dataRoot)
      .filter((path) => path.endsWith('.service.ts'))
      .map((path) => readFileSync(path, 'utf8'))
      .join('\n');

    expect(serviceSource).toContain('DashboardBackendService');
    expect(serviceSource).not.toMatch(/\bHttpClient\b/);
  });
});
