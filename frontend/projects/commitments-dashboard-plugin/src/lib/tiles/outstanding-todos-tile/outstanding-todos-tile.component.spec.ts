import { readFileSync } from 'fs';
import { join } from 'path';

describe('OutstandingTodosTileComponent (template + CSS source)', () => {
  const html = readFileSync(
    join(__dirname, 'outstanding-todos-tile.component.html'),
    'utf8'
  );
  const scss = readFileSync(
    join(__dirname, 'outstanding-todos-tile.component.scss'),
    'utf8'
  );

  function ruleBlock(selector: string): string {
    const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
    return match ? match[1] : '';
  }

  it('wraps count and copy in a horizontal .todo-body row (bug-026)', () => {
    expect(html).toMatch(/class="todo-body"/);

    const block = ruleBlock('\\.todo-body');
    expect(block).toMatch(/display\s*:\s*flex\b/);
    expect(block).toMatch(/align-items\s*:\s*center\b/);
  });
});
