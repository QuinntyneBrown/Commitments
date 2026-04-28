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

  it('keeps every template line under 110 characters (bug-073)', () => {
    const overLong = html.split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, len: line.length }))
      .filter(({ len }) => len > 110);
    expect(overLong).toEqual([]);
  });

  it('e2e POM + spec match the renamed title — no stale "Outstanding To-Dos" (bug-074)', () => {
    const pom = readFileSync(
      join(__dirname, '../../../../../commitments-app/e2e/pages/dashboard.page.ts'),
      'utf8'
    );
    const spec = readFileSync(
      join(__dirname, '../../../../../commitments-app/e2e/dashboard.spec.ts'),
      'utf8'
    );
    // Neither file may reference "Outstanding To-Dos" (regular hyphen)
    // nor "Outstanding To‑Dos" (U+2011 non-breaking hyphen).
    expect(pom).not.toMatch(/Outstanding To.Dos/);
    expect(spec).not.toMatch(/Outstanding To.Dos/);
  });

  it('component metadata displayName matches the renamed title (bug-081)', () => {
    const ts = readFileSync(
      join(__dirname, 'outstanding-todos-tile.component.ts'),
      'utf8'
    );
    // displayName is the source of truth for the add-tile dialog.
    // It must match the catalog and tile-shell title — no stale
    // "Outstanding To-Dos" or U+2011 "Outstanding To‑Dos" form.
    expect(ts).not.toMatch(/Outstanding To.Dos/);
  });
});
