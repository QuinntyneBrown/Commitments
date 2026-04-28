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

  it('uses the .pen-aligned title and eyebrow strings (bug-027)', () => {
    expect(html).toMatch(/title="Outstanding Todos"/);
    expect(html).toMatch(/eyebrow="Tasks"/);
    // U+2011 non-breaking hyphen must not appear in the template
    expect(html).not.toContain('‑');
  });

  it('returns the warning pill variant in live mode (bug-035)', () => {
    const ts = readFileSync(
      join(__dirname, 'outstanding-todos-tile.component.ts'),
      'utf8'
    );
    expect(ts).toMatch(/pillVariant\b[\s\S]*?'warning'/);
  });

  it('renders the count at weight 700 (bug-038)', () => {
    const block = ruleBlock('\\.todo-count');
    expect(block).toMatch(/font-weight\s*:\s*700\b/);
  });

  it('renders the copy at 12px (bug-038)', () => {
    const block = ruleBlock('\\.todo-copy');
    expect(block).toMatch(/font-size\s*:\s*12px\b/);
  });

  it('opts in to the warning-amber icon colour (bug-048)', () => {
    const block = ruleBlock(':host');
    expect(block).toMatch(/--cui-tile-icon-color\s*:[^;]*--cui-warning/);
  });

  it('projects the status-pill into the tile-shell header (bug-057)', () => {
    expect(html).toMatch(/<cui-status-pill\b[^>]*\btile-status\b/);
  });
});
