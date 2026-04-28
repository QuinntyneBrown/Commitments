import { readFileSync } from 'fs';
import { join } from 'path';

describe('ReviewScrubberComponent (CSS source)', () => {
  const scss = readFileSync(
    join(__dirname, 'review-scrubber.component.scss'),
    'utf8'
  );

  it('every var(--cui-*) reference includes a fallback hex (bug-107)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });
});

describe('ReviewScrubberComponent (TS source)', () => {
  const ts = readFileSync(
    join(__dirname, 'review-scrubber.component.ts'),
    'utf8'
  );

  it('migrates @Input to input() with reactive forward (bug-128)', () => {
    expect(ts).not.toMatch(/@Input\b/);
    expect(ts).toMatch(/import\s*\{[^}]*\binput\b[^}]*\}\s*from\s*'@angular\/core'/);
    expect(ts).toMatch(/\bwindowDays\s*=\s*input\b/);
    // The forward to controller.windowDays.set must run inside an
    // effect() so input changes propagate after init.
    expect(ts).toMatch(/effect\(\s*\(\)\s*=>\s*\{[\s\S]*?windowDays\(\)[\s\S]*?controller\.windowDays\.set/);
  });
});
