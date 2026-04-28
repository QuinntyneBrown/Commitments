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
});
