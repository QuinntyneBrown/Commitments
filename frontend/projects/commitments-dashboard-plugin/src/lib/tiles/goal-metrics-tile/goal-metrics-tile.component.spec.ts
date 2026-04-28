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

  it('keeps every template line under 110 characters (bug-073)', () => {
    const overLong = html.split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, len: line.length }))
      .filter(({ len }) => len > 110);
    expect(overLong).toEqual([]);
  });
});
