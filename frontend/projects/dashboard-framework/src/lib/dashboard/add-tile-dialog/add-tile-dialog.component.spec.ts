import { readFileSync } from 'fs';
import { join } from 'path';

describe('AddTileDialogComponent (CSS source)', () => {
  const scss = readFileSync(
    join(__dirname, 'add-tile-dialog.component.scss'),
    'utf8'
  );

  it('every var(--cui-*) reference includes a fallback hex (bug-105)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });

  it('font-family declarations reference the --cui-font-display token (bug-173)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /\bfont-family\s*:\s*Inter\b/.test(line));
    expect(offenders).toEqual([]);
  });
});
