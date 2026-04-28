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
});
