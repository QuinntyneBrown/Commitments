import { readFileSync } from 'fs';
import { join } from 'path';

describe('RelationsTileComponent (template + CSS source)', () => {
  const html = readFileSync(
    join(__dirname, 'relations-tile.component.html'),
    'utf8'
  );
  const scss = readFileSync(
    join(__dirname, 'relations-tile.component.scss'),
    'utf8'
  );

  function ruleBlock(selector: string): string {
    const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
    return match ? match[1] : '';
  }

  it('wraps each relation in a .relations__row div (bug-028)', () => {
    expect(html).toMatch(/class="relations__row"/);
  });

  it('renders relations body at 14px and vertically centered (bug-028)', () => {
    const block = ruleBlock('\\.relations');
    expect(block).toMatch(/font-size\s*:\s*14px\b/);
    expect(block).toMatch(/justify-content\s*:\s*center\b/);
  });

  it('uses --cui-divider-soft (#2A2A2A) bottom dividers between rows (bug-028, post bug-066)', () => {
    const block = ruleBlock('\\.relations__row');
    expect(block).toMatch(/border-bottom\s*:[^;]*(?:#2A2A2A|--cui-divider-soft)/i);
  });

  it('references --cui-divider-soft token for the row divider (bug-066)', () => {
    const block = ruleBlock('\\.relations__row');
    expect(block).toMatch(/--cui-divider-soft\b/);
  });

  it('opts in to the 12px tile-body gap (bug-041)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-body-gap\s*:\s*12px\b/);
  });

  it('does not project a status-pill — design forbids one (bug-078)', () => {
    expect(html).not.toMatch(/<cui-status-pill\b/);
  });

  it('keeps every template line under 110 characters (bug-073)', () => {
    const overLong = html.split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, len: line.length }))
      .filter(({ len }) => len > 110);
    expect(overLong).toEqual([]);
  });

  it('every var(--cui-*) reference includes a fallback hex (bug-099)', () => {
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });
});
