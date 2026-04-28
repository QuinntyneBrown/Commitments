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

  it('uses inject(WEEKLY_FOCUS_BASE_URL, { optional: true }) for the base URL (bug-137)', () => {
    expect(src).toMatch(
      /private\s+readonly\s+_baseUrl\s*=\s*inject\(\s*WEEKLY_FOCUS_BASE_URL\s*,\s*\{\s*optional:\s*true\s*\}\s*\)/
    );
  });
});
