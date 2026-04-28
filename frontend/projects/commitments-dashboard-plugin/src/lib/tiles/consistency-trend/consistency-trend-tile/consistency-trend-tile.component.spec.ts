import { readFileSync } from 'fs';
import { join } from 'path';

describe('ConsistencyTrendTileComponent (template + CSS source)', () => {
  const html = readFileSync(
    join(__dirname, 'consistency-trend-tile.component.html'),
    'utf8'
  );
  const scss = readFileSync(
    join(__dirname, 'consistency-trend-tile.component.scss'),
    'utf8'
  );

  function ruleBlock(selector: string): string {
    const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
    return match ? match[1] : '';
  }

  it('co-locates metric header and delta badge in .trend-row (bug-029)', () => {
    const trendRowMatch = html.match(/<div class="trend-row"[^>]*>([\s\S]*?)<\/div>/);
    expect(trendRowMatch).toBeTruthy();
    const inner = trendRowMatch![1];
    expect(inner).toMatch(/<cui-metric-header\b/);
    expect(inner).toMatch(/<cui-delta-badge\b/);
  });

  it('lays .trend-row out as an end-aligned space-between flex row (bug-029)', () => {
    const block = ruleBlock('\\.trend-row');
    expect(block).toMatch(/display\s*:\s*flex\b/);
    expect(block).toMatch(/align-items\s*:\s*flex-end\b/);
    expect(block).toMatch(/justify-content\s*:\s*space-between\b/);
  });

  it('returns the chart pill variant in live mode (bug-031)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // Match either `pillVariant() { return ... 'chart' }` or
    // `pillVariant = computed(() => ... 'chart')` (post bug-061).
    expect(ts).toMatch(/pillVariant\b[\s\S]*?'chart'/);
  });

  it('passes today caption + Peak/Low subCaption to the metric header (bug-032)', () => {
    expect(html).toMatch(/caption="today"/);
    expect(html).toMatch(/\[subCaption\]\s*=\s*"'Peak/);
  });

  it('aligns trend-row with chart-variant body padding-top (bug-040)', () => {
    const block = ruleBlock('\\.trend-row');
    // .trend-row should not add its own margin-top now that the
    // tile-shell chart variant supplies the 14px gap from above.
    expect(block).not.toMatch(/margin-top\s*:/);
  });

  it('uses 14px gap above the chart canvas (bug-040)', () => {
    const block = ruleBlock('\\.plot');
    expect(block).toMatch(/margin-top\s*:\s*14px\b/);
  });

  it('opts in to the 14px tile-body gap (bug-041)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-body-gap\s*:\s*14px\b/);
  });

  it('configures the Y-axis grid as muted white (bug-043)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The chart Y-grid colour should now be a muted white rgba —
    // not the legacy #3A3A3A divider gray.
    expect(ts).toMatch(/grid\s*:\s*\{[^}]*color\s*:\s*'rgba\(\s*255\s*,\s*255\s*,\s*255/);
    expect(ts).not.toMatch(/grid\s*:\s*\{[^}]*color\s*:\s*'#3A3A3A'/i);
  });

  it('formats x-axis labels with toLocaleDateString month/day (bug-046)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The x-axis tick callback should render labels as "Apr 25" via
    // toLocaleDateString('en-US', { month: 'short', day: 'numeric' }).
    expect(ts).toMatch(/callback\b[\s\S]*?toLocaleDateString[\s\S]*?'short'[\s\S]*?'numeric'/);
  });

  it('highlights the last x-axis tick in chart accent + weight 700 (bug-047)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // x-axis tick color must be a Scriptable function that references
    // ACCENT_CHART; tick font must include weight 700 conditionally.
    expect(ts).toMatch(/color\s*:\s*\([^)]*\)\s*=>[\s\S]*?ACCENT_CHART/);
    expect(ts).toMatch(/font\s*:\s*\([^)]*\)\s*=>[\s\S]*?\b700\b/);
  });

  it('opts in to the info-blue icon colour (bug-048)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-icon-color\s*:[^;]*--cui-info/);
  });

  it('opts in to the 22px icon size (bug-049)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-icon-size\s*:\s*22px\b/);
  });

  it('opts in to the 10px header gap (bug-050)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-header-gap\s*:\s*10px\b/);
  });

  it('uses signal-form input() for goalId and windowDays (bug-071)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).not.toMatch(/@Input\b/);
    expect(ts).toMatch(/\bgoalId\s*=\s*input\(/);
    expect(ts).toMatch(/\bwindowDays\s*=\s*input\(/);
  });

  it('keeps every template line under 110 characters (bug-070)', () => {
    const lines = html.split(/\r?\n/);
    const overLong = lines
      .map((line, i) => ({ n: i + 1, len: line.length }))
      .filter(({ len }) => len > 110);
    expect(overLong).toEqual([]);
  });

  it('binds the delta-badge to parsed delta + caption (bug-055)', () => {
    expect(html).toMatch(/\[delta\]\s*=\s*"controller\.deltaPercentage\(\)"/);
    expect(html).toMatch(/\[caption\]\s*=\s*"controller\.deltaCaption\(\)"/);
    expect(html).not.toMatch(/controller\.currentPercentage\(\)\s*-\s*controller\.lowPercentage\(\)/);
  });

  it('sets data-testid="consistency-trend-tile" on the tile-shell (bug-056)', () => {
    expect(html).toMatch(/<commitments-tile-shell\b[^>]*data-testid="consistency-trend-tile"/);
  });

  it('projects the status-pill into the tile-shell header (bug-057)', () => {
    expect(html).toMatch(/<cui-status-pill\b[^>]*\btile-status\b/);
  });

  it('uses computed signals for mode/pillVariant/statusLabel (bug-061)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).toMatch(/\bmode\s*=\s*computed\(/);
    expect(ts).toMatch(/\bpillVariant\s*=\s*computed</);
    expect(ts).toMatch(/\bstatusLabel\s*=\s*computed\(/);
    // The old method-form pillLabel must be gone.
    expect(ts).not.toMatch(/\bpillLabel\s*\(\s*\)\s*:/);
  });

  it('binds the pill label to statusLabel() (bug-061)', () => {
    expect(html).toMatch(/\[label\]\s*=\s*"statusLabel\(\)"/);
    expect(html).not.toMatch(/\[label\]\s*=\s*"pillLabel\(\)"/);
  });
});
