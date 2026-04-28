import { readFileSync } from 'fs';
import { join } from 'path';

describe('DailyResultsTileComponent (CSS source)', () => {
  const scss = readFileSync(
    join(__dirname, 'daily-results-tile.component.scss'),
    'utf8'
  );

  function ruleBlock(selector: string): string {
    const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
    return match ? match[1] : '';
  }

  it('centers .metric body content (bug-021)', () => {
    const block = ruleBlock('\\.metric');
    expect(block).toMatch(/display\s*:\s*flex\b/);
    expect(block).toMatch(/justify-content\s*:\s*center\b/);
    expect(block).toMatch(/align-items\s*:\s*center\b/);
  });

  it('renders the metric value at 48px / weight 700 (bug-036)', () => {
    const block = ruleBlock('\\.metric__value');
    expect(block).toMatch(/font-size\s*:\s*48px\b/);
    expect(block).toMatch(/font-weight\s*:\s*700\b/);
  });

  it('renders the metric label at 12px (bug-036)', () => {
    const block = ruleBlock('\\.metric__label');
    expect(block).toMatch(/font-size\s*:\s*12px\b/);
  });

  it('progress bar margin matches the design root gap of 10px (bug-085)', () => {
    const block = ruleBlock('\\.progress');
    // Either the literal 10px or the --cui-tile-body-gap token (default 10px).
    expect(block).toMatch(/margin-top\s*:\s*(?:10px|var\(\s*--cui-tile-body-gap)/);
    expect(block).not.toMatch(/margin-top\s*:\s*18px\b/);
  });

  it('progress bar uses the --cui-radius-full token, not literal 999px (bug-095)', () => {
    const block = ruleBlock('\\.progress');
    expect(block).toMatch(/border-radius\s*:\s*var\(\s*--cui-radius-full/);
    expect(block).not.toMatch(/border-radius\s*:\s*999px\b/);
  });
});

describe('DailyResultsTileComponent (TS source)', () => {
  const ts = readFileSync(
    join(__dirname, 'daily-results-tile.component.ts'),
    'utf8'
  );

  it('returns the success pill variant in live mode (bug-033)', () => {
    // Match either `pillVariant() { return ... 'success' }` or
    // `pillVariant = computed(() => ... 'success')`
    expect(ts).toMatch(/pillVariant\b[\s\S]*?'success'/);
  });
});

describe('DailyResultsTileComponent (template line length)', () => {
  it('keeps every template line under 110 characters (bug-073)', () => {
    const html = readFileSync(
      join(__dirname, 'daily-results-tile.component.html'),
      'utf8'
    );
    const overLong = html.split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, len: line.length }))
      .filter(({ len }) => len > 110);
    expect(overLong).toEqual([]);
  });
});
