import { readFileSync } from 'fs';
import { join } from 'path';

describe('MonthlyProgressTileComponent (template source)', () => {
  const html = readFileSync(
    join(__dirname, 'monthly-progress-tile.component.html'),
    'utf8'
  );

  it('renders a week-label row beside the bars (bug-024)', () => {
    expect(html).toMatch(/class="bar-labels"/);
    expect(html).toMatch(/@for\s*\([^)]*controller\.buckets\(\)/);
  });

  it('does not project a status-pill — design forbids one (bug-076)', () => {
    expect(html).not.toMatch(/<cui-status-pill\b/);
  });

  it('keeps every template line under 110 characters (bug-073)', () => {
    const overLong = html.split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, len: line.length }))
      .filter(({ len }) => len > 110);
    expect(overLong).toEqual([]);
  });
});

describe('MonthlyProgressTileComponent (CSS source)', () => {
  const scss = readFileSync(
    join(__dirname, 'monthly-progress-tile.component.scss'),
    'utf8'
  );

  function ruleBlock(selector: string): string {
    const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
    return match ? match[1] : '';
  }

  it('uses 4px top-corner radius on bars (bug-025)', () => {
    const block = ruleBlock('\\.bars span');
    expect(block).toMatch(/border-radius\s*:\s*4px\s+4px\s+0\s+0\b/);
  });

  it('uses 12px gap on the bars row (bug-025)', () => {
    const block = ruleBlock('\\.bars');
    expect(block).toMatch(/gap\s*:\s*12px\b/);
  });

  it('uses 12px gap on the bar-labels row (bug-025)', () => {
    const block = ruleBlock('\\.bar-labels');
    expect(block).toMatch(/gap\s*:\s*12px\b/);
  });

  it('insets bars horizontally to align with the bar-labels row (bug-039)', () => {
    const block = ruleBlock('\\.bars');
    expect(block).toMatch(/padding\s*:[^;]*\b4px\b/);
  });

  it('opts in to the 12px tile-body gap (bug-041)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-body-gap\s*:\s*12px\b/);
  });

  it('opts in to the info-blue icon colour (bug-048)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-icon-color\s*:[^;]*--cui-info/);
  });

  it('uses --cui-text-disabled token for bar labels (bug-065)', () => {
    const block = ruleBlock('\\.bar-labels');
    expect(block).toMatch(/var\(\s*--cui-text-disabled/);
    expect(block).not.toMatch(/color\s*:\s*#666666\b/i);
  });

  it('every var(--cui-*) reference includes a fallback hex (bug-098)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });

  it('bar gradient is lighter at top, darker at bottom per design (bug-120)', () => {
    const block = ruleBlock('\\.bars span');
    // Design screenshot: bars are bright #8bb8e8 at top, deeper
    // #2f6db3 at the bottom. Impl had it reversed; the lighter
    // shade must come first in the linear-gradient (180deg).
    expect(block).toMatch(/linear-gradient\(180deg,\s*#8bb8e8,\s*#2f6db3\)/i);
    expect(block).not.toMatch(/linear-gradient\(180deg,\s*#2f6db3,\s*#8bb8e8\)/i);
  });
});
