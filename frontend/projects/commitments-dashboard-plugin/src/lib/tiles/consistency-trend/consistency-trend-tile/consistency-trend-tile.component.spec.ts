import { readFileSync } from 'fs';
import { join } from 'path';

describe('ConsistencyTrendTileComponent (single fetch effect, source)', () => {
  const ts = readFileSync(
    join(__dirname, 'consistency-trend-tile.component.ts'),
    'utf8'
  );

  it('does not call controller.refresh() from the tile (bug-142)', () => {
    expect(ts).not.toMatch(/this\.controller\.refresh\(/);
  });

  it('reads tile-context mode/asOf inside the goalId/load effect (bug-142)', () => {
    // The effect that calls controller.load() must also subscribe
    // to mode/asOf signals so a single effect handles every fetch
    // trigger. Match the effect block that contains
    // `controller.load(` and assert it also reads
    // `_tileContext?.mode()` and `_tileContext?.selectedReviewDate()`.
    const match = ts.match(/effect\(\s*\(\)\s*=>\s*\{([\s\S]*?)\bthis\.controller\.load\([\s\S]*?\}\s*\)\s*;/);
    expect(match).not.toBeNull();
    const body = match![0];
    expect(body).toMatch(/this\._tileContext\?\.mode\(\)/);
    expect(body).toMatch(/this\._tileContext\?\.selectedReviewDate\(\)/);
  });
});

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

  it('caps the Y-axis at 5 grid lines to match the design (bug-091)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The Y axis block must include `maxTicksLimit: 5` since labels
    // are hidden — the tick count drives the grid line count.
    expect(ts).toMatch(/y\s*:\s*\{[\s\S]*?maxTicksLimit\s*:\s*5\b/);
  });

  it('pads x-axis tick labels 6px below the plot (bug-093)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The X axis ticks block must set `padding: 6` to match the
    // design's chart-frame gap of 6 between plot and labels.
    expect(ts).toMatch(/x\s*:\s*\{[\s\S]*?ticks\s*:\s*\{[\s\S]*?padding\s*:\s*6\b/);
  });

  it('caps the x-axis at 5 date labels to match the design (bug-094)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The design's xLabRow has exactly 5 dates — match it.
    expect(ts).toMatch(/x\s*:\s*\{[\s\S]*?ticks\s*:\s*\{[\s\S]*?maxTicksLimit\s*:\s*5\b/);
    expect(ts).not.toMatch(/x\s*:\s*\{[\s\S]*?ticks\s*:\s*\{[\s\S]*?maxTicksLimit\s*:\s*6\b/);
  });

  it('every var(--cui-*) reference includes a fallback hex (bug-101)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });

  it('draws a glow behind the highlighted today-point (bug-114)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The chart config must register a plugin that uses
    // shadowBlur + shadowColor to draw the glow at
    // controller.highlightedIndex() — matching the design's
    // todayDot effect (blur 8, #42A5F5CC).
    expect(ts).toMatch(/plugins\s*:\s*\[[\s\S]*?todayPointGlow/);
    expect(ts).toMatch(/shadowBlur\s*=\s*\S+/);
    expect(ts).toMatch(/shadowColor\b[\s\S]*?ACCENT_CHART/);
    expect(ts).toMatch(/highlightedIndex\(\)/);
  });

  it('types the todayPointGlow plugin as Plugin<line> (bug-115)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).toMatch(/import\s*\{[^}]*\bPlugin\b[^}]*\}\s*from\s*'chart\.js'/);
    expect(ts).toMatch(/todayPointGlow\s*:\s*Plugin<'line'>/);
  });

  it('todayPointGlow uses named constants for radius + blur (bug-116)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).toMatch(/TODAY_GLOW_RADIUS\s*=\s*7\b/);
    expect(ts).toMatch(/TODAY_GLOW_BLUR\s*=\s*8\b/);
    // Plugin body must reference the constants, not literal 7/8.
    expect(ts).toMatch(/shadowBlur\s*=\s*TODAY_GLOW_BLUR/);
    expect(ts).toMatch(/ctx\.arc\([^)]*TODAY_GLOW_RADIUS/);
  });

  it('hides the X and Y axis borders (bug-117)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).toMatch(/x\s*:\s*\{[\s\S]*?border\s*:\s*\{\s*display\s*:\s*false/);
    expect(ts).toMatch(/y\s*:\s*\{[\s\S]*?border\s*:\s*\{\s*display\s*:\s*false/);
  });

  it('extracts the tick highlight predicate to one place (bug-118)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The inline predicate `ctx.index === controller.highlightedIndex()`
    // should appear AT MOST ONCE — the helper definition. The two
    // tick Scriptables (color + font) call the helper instead.
    const matches = ts.match(/ctx\.index\s*===\s*controller\.highlightedIndex\(\)/g) ?? [];
    expect(matches.length).toBeLessThanOrEqual(1);
    expect(ts).toMatch(/isHighlighted\b/);
  });

  it('extracts chart config building to a private method (bug-119)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // ngAfterViewInit body should be brief — most of the chart-config
    // glue moves into `_buildChartConfig`.
    const ngAfterViewInitMatch = ts.match(/ngAfterViewInit\(\)\s*:\s*void\s*\{([\s\S]*?)\n  \}/);
    expect(ngAfterViewInitMatch).toBeTruthy();
    const body = ngAfterViewInitMatch![1];
    const lineCount = body.split('\n').filter(l => l.trim().length > 0).length;
    expect(lineCount).toBeLessThanOrEqual(5);
    expect(ts).toMatch(/_buildChartConfig\b/);
  });

  it('eyebrow tracks windowDays input rather than a literal "14 days" (bug-122)', () => {
    expect(html).toMatch(/\[eyebrow\]\s*=\s*"[^"]*windowDays\(\)/);
    expect(html).not.toMatch(/eyebrow="7-day rolling average · last 14 days"/);
  });

  it('reactively loads on goalId / windowDays input change (bug-123)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // Find the effect block that contains controller.load and verify
    // it references both goalId() and windowDays() (order-agnostic).
    const effectWithLoad = ts.match(/effect\(\s*\(\)\s*=>\s*\{([\s\S]*?controller\.load[\s\S]*?)\}\s*\)/);
    expect(effectWithLoad).toBeTruthy();
    const body = effectWithLoad![1];
    expect(body).toMatch(/goalId\(\)/);
    expect(body).toMatch(/windowDays\(\)/);
    // The legacy ngOnInit-based load should be gone.
    expect(ts).not.toMatch(/ngOnInit\(\)\s*:\s*void\s*\{[^}]*controller\.load/);
  });

  it('migrates @ViewChild to signal-based viewChild() (bug-124)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).not.toMatch(/@ViewChild\b/);
    expect(ts).toMatch(/import\s*\{[^}]*\bviewChild\b[^}]*\}\s*from\s*'@angular\/core'/);
    expect(ts).toMatch(/plotRef\s*=\s*viewChild\b/);
  });

  it('fallback mode/date use computed(), not signal().asReadonly() (bug-133)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).not.toMatch(/signal[^;]*\.asReadonly\(\)/);
    expect(ts).toMatch(/_fallbackMode\s*=\s*computed</);
    expect(ts).toMatch(/_fallbackDate\s*=\s*computed</);
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

  it('tick label highlight tracks controller.highlightedIndex, not isLastTick (bug-092)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    // The tick color/font Scriptables must drive their highlight from
    // the controller's highlightedIndex so review mode highlights the
    // selected date's label, not always the last. After bug-118 the
    // predicate is extracted to an isHighlighted helper; either form
    // is acceptable as long as the Scriptables don't reference
    // isLastTick.
    expect(ts).toMatch(/color\s*:\s*\([^)]*\)\s*=>[\s\S]*?(highlightedIndex\(\)|isHighlighted\()/);
    expect(ts).toMatch(/font\s*:\s*\([^)]*\)\s*=>[\s\S]*?(highlightedIndex\(\)|isHighlighted\()/);
    // The isLastTick helper has no remaining callers and should be gone.
    expect(ts).not.toMatch(/isLastTick\b/);
  });

  it('non-highlighted tick color uses TEXT_MUTED token, not literal #666666 (bug-088)', () => {
    const ts = readFileSync(
      join(__dirname, 'consistency-trend-tile.component.ts'),
      'utf8'
    );
    expect(ts).toMatch(/import\s*\{[^}]*\bTEXT_MUTED\b[^}]*\}\s*from\s*'@commitments\/ui'/);
    expect(ts).toMatch(/color\s*:\s*\([^)]*\)\s*=>[\s\S]*?TEXT_MUTED/);
    expect(ts).not.toMatch(/'#666666'/);
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

  it('opts in to 20px outer padding to match the .pen design (bug-087)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-padding\s*:\s*20px\b/);
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
