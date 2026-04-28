import { readFileSync } from 'fs';
import { join } from 'path';

describe('DashboardGridComponent (CSS source)', () => {
  const scss = readFileSync(
    join(__dirname, 'dashboard-grid.component.scss'),
    'utf8'
  );

  it('every var(--cui-*) reference includes a fallback hex (bug-103)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });

  it('edit-mode border-radius uses --cui-radius-md token (bug-184)', () => {
    const block = scss.match(/\.dashboard-grid--edit-mode gridster-item\s*\{([^}]*)\}/);
    expect(block).not.toBeNull();
    expect(block![1]).toMatch(/border-radius\s*:\s*var\(\s*--cui-radius-md/);
  });
});
