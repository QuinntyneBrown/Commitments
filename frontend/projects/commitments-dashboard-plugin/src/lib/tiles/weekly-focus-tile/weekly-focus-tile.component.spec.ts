import { readFileSync } from 'fs';
import { join } from 'path';

describe('WeeklyFocusTileComponent (CSS source)', () => {
  const scss = readFileSync(
    join(__dirname, 'weekly-focus-tile.component.scss'),
    'utf8'
  );

  function ruleBlock(selector: string): string {
    const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
    return match ? match[1] : '';
  }

  it('stacks each focus row vertically (bug-022)', () => {
    const block = ruleBlock('\\.focus-list li');
    expect(block).toMatch(/flex-direction\s*:\s*column\b/);
  });

  it('renders supporting metric at 11px (bug-023)', () => {
    const block = ruleBlock('\\.focus-list span');
    expect(block).toMatch(/font-size\s*:\s*11px\b/);
  });

  it('uses --cui-divider-soft (#2A2A2A) for the row divider (bug-023, post bug-066)', () => {
    const block = ruleBlock('\\.focus-list li');
    // Tolerant of either the literal `#2A2A2A` form or
    // `var(--cui-divider-soft, …)` token form.
    expect(block).toMatch(/border-bottom\s*:[^;]*(?:#2A2A2A|--cui-divider-soft)/i);
  });

  it('references --cui-divider-soft token for the row divider (bug-066)', () => {
    const block = ruleBlock('\\.focus-list li');
    expect(block).toMatch(/--cui-divider-soft\b/);
  });

  it('pins focus name typography to 14px / 700 (bug-037)', () => {
    const block = ruleBlock('\\.focus-list strong');
    expect(block).toMatch(/font-size\s*:\s*14px\b/);
    expect(block).toMatch(/font-weight\s*:\s*700\b/);
  });

  it('opts in to the 12px tile-body gap (bug-041)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-body-gap\s*:\s*12px\b/);
  });

  it('does not project a status-pill — design forbids one (bug-080)', () => {
    const html = readFileSync(
      join(__dirname, 'weekly-focus-tile.component.html'),
      'utf8'
    );
    expect(html).not.toMatch(/<cui-status-pill\b/);
  });

  it('keeps every template line under 110 characters (bug-073)', () => {
    const html = readFileSync(
      join(__dirname, 'weekly-focus-tile.component.html'),
      'utf8'
    );
    const overLong = html.split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, len: line.length }))
      .filter(({ len }) => len > 110);
    expect(overLong).toEqual([]);
  });

  it('every var(--cui-*) reference includes a fallback hex (bug-100)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });
});
