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
      .filter(({ line }) => /var\(--cui-[a-z-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });
});
