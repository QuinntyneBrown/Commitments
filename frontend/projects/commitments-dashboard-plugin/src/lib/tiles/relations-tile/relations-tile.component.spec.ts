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

  it('projects the status-pill into the tile-shell header (bug-057)', () => {
    const html = readFileSync(
      join(__dirname, 'relations-tile.component.html'),
      'utf8'
    );
    expect(html).toMatch(/<cui-status-pill\b[^>]*\btile-status\b/);
  });
});
