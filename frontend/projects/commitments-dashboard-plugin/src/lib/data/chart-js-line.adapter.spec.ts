import { readFileSync } from 'fs';
import { join } from 'path';

describe('chart-js-line.adapter (source)', () => {
  const ts = readFileSync(
    join(__dirname, 'chart-js-line.adapter.ts'),
    'utf8'
  );

  it('does not export the dead buildVerticalGradient helper (bug-060)', () => {
    expect(ts).not.toMatch(/buildVerticalGradient\b/);
  });
});
