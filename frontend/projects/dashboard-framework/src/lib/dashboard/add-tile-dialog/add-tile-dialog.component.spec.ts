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

  it('--cui-on-primary fallback matches the canonical token value #1A1B23 (bug-176)', () => {
    expect(scss).not.toMatch(/var\(--cui-on-primary,\s*#fff\)/);
    expect(scss).toMatch(/var\(\s*--cui-on-primary\s*,\s*#1A1B23\s*\)/);
  });
});
