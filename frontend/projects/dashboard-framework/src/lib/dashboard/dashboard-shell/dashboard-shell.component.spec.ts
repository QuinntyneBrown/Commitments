import { readFileSync } from 'fs';
import { join } from 'path';

describe('DashboardShellComponent (CSS source)', () => {
  const scss = readFileSync(
    join(__dirname, 'dashboard-shell.component.scss'),
    'utf8'
  );

  it('every var(--cui-*) reference includes a fallback hex (bug-106)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });

  it('font-family declarations reference the --cui-font-display token (bug-172)', () => {
    // No bare `font-family: Inter,` literals — every declaration
    // should route through the design token (with Inter still
    // present as the fallback inside `var(--cui-font-display, ...)`).
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /\bfont-family\s*:\s*Inter\b/.test(line));
    expect(offenders).toEqual([]);
  });
});
