import { readFileSync } from 'fs';
import { join } from 'path';

describe('GoalMetricsTileComponent (template source)', () => {
  const html = readFileSync(
    join(__dirname, 'goal-metrics-tile.component.html'),
    'utf8'
  );

  it('projects the status-pill into the tile-shell header (bug-058)', () => {
    expect(html).toMatch(/<cui-status-pill\b[^>]*\btile-status\b/);
  });
});
