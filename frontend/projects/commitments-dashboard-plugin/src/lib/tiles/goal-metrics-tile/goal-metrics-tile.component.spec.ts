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

  it('e2e dashboard.spec matches the merged tile name — no stale "Live Goal Metrics" (bug-084)', () => {
    const spec = readFileSync(
      join(__dirname, '../../../../../commitments-app/e2e/dashboard.spec.ts'),
      'utf8'
    );
    // design-07 merged "Live Goal Metrics" + "Review Goal History"
    // into a single dual-mode "Goal Metrics" tile. The e2e catalog
    // assertion must use the merged displayName.
    expect(spec).not.toMatch(/Live Goal Metrics/);
  });

  it('every var(--cui-*) reference includes a fallback hex (bug-102)', () => {
    const scss = readFileSync(
      join(__dirname, 'goal-metrics-tile.component.scss'),
      'utf8'
    );
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });
});
