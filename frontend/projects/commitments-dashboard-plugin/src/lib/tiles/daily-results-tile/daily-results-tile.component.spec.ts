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
