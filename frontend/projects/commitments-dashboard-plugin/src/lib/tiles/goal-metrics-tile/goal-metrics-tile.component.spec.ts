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

  it('migrates @Input setter to input() signal + effect (bug-072)', () => {
    const ts = readFileSync(
      join(__dirname, 'goal-metrics-tile.component.ts'),
      'utf8'
    );
    expect(ts).not.toMatch(/@Input\b/);
    expect(ts).toMatch(/\bgoalId\s*=\s*input\(/);
    expect(ts).toMatch(/effect\(\s*\(\)\s*=>[\s\S]*?setGoalId\(\s*this\.goalId\(\)/);
  });
});
