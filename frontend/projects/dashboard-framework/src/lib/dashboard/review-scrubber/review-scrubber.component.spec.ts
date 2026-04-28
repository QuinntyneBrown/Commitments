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
