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
