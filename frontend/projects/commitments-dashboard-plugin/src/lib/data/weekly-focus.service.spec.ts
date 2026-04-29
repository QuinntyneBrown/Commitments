import { readFileSync } from 'fs';
import { join } from 'path';

describe('WeeklyFocusService (DI shape, source)', () => {
  const src = readFileSync(
    join(__dirname, 'weekly-focus.service.ts'),
    'utf8'
  );

  it('does not import @Inject or @Optional decorators (bug-137)', () => {
    const importLine = src.match(/import\s*\{([^}]*)\}\s*from\s*'@angular\/core'/);
    expect(importLine).not.toBeNull();
    const names = importLine![1];
    expect(names).not.toMatch(/\bInject\b(?!ionToken|able)/);
    expect(names).not.toMatch(/\bOptional\b/);
  });

  it('declares no constructor (bug-137)', () => {
    expect(src).not.toMatch(/\bconstructor\s*\(/);
  });

  it('delegates backend communication to DashboardBackendService', () => {
    expect(src).toContain('DashboardBackendService');
    expect(src).toMatch(/private\s+readonly\s+_backend\s*=\s*inject\(\s*DashboardBackendService\s*\)/);
    expect(src).not.toContain('HttpClient');
  });
});
