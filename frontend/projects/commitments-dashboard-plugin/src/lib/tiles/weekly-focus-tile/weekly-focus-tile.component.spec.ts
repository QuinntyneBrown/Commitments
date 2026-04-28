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

  it('uses #2A2A2A for the row divider (bug-023)', () => {
    const block = ruleBlock('\\.focus-list li');
    expect(block).toMatch(/border-bottom\s*:[^;]*#2A2A2A/i);
  });
});
