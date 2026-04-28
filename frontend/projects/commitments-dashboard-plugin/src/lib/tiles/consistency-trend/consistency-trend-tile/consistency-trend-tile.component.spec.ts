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
    // pillVariant() should map live mode to 'chart' for the chart-themed tile
    expect(ts).toMatch(/pillVariant\s*\([^)]*\)[^{]*\{[\s\S]*?return[^;]*'chart'/);
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
});
