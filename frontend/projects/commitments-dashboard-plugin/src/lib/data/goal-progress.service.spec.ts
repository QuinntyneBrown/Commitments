import { readFileSync } from 'fs';
import { join } from 'path';

describe('GoalProgressService (DI shape, source)', () => {
  const src = readFileSync(
    join(__dirname, 'goal-progress.service.ts'),
    'utf8'
  );

  it('does not import @Inject or @Optional decorators (bug-140)', () => {
    const importLine = src.match(/import\s*\{([^}]*)\}\s*from\s*'@angular\/core'/);
    expect(importLine).not.toBeNull();
    const names = importLine![1];
    expect(names).not.toMatch(/\bInject\b(?!ionToken|able)/);
    expect(names).not.toMatch(/\bOptional\b/);
  });

  it('declares no constructor (bug-140)', () => {
    expect(src).not.toMatch(/\bconstructor\s*\(/);
  });

  it('uses inject(GOAL_PROGRESS_BASE_URL, { optional: true }) for the base URL (bug-140)', () => {
    expect(src).toMatch(
      /private\s+readonly\s+_baseUrl\s*=\s*inject\(\s*GOAL_PROGRESS_BASE_URL\s*,\s*\{\s*optional:\s*true\s*\}\s*\)/
    );
  });
});
