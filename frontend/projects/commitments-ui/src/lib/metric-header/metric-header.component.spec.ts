import { TestBed } from '@angular/core/testing';
import { readFileSync } from 'fs';
import { join } from 'path';

import { MetricHeaderComponent } from './metric-header.component';

describe('MetricHeaderComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [MetricHeaderComponent] });
  });

  it('defaults to chart accent and empty caption', () => {
    const component = TestBed.createComponent(MetricHeaderComponent).componentInstance;
    expect(component.value()).toBe('');
    expect(component.caption()).toBe('');
    expect(component.accent()).toBe('chart');
  });

  it('exposes a subCaption input that defaults to empty (bug-032)', () => {
    const component = TestBed.createComponent(MetricHeaderComponent).componentInstance;
    expect(component.subCaption()).toBe('');
  });

  describe('CSS source', () => {
    const scss = readFileSync(
      join(__dirname, 'metric-header.component.scss'),
      'utf8'
    );

    function ruleBlock(selector: string): string {
      const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
      return match ? match[1] : '';
    }

    it('renders the value at 48px / weight 700 (bug-030)', () => {
      const block = ruleBlock('\\.metric-header__value');
      expect(block).toMatch(/font-size\s*:\s*48px\b/);
      expect(block).toMatch(/font-weight\s*:\s*700\b/);
    });

    it('lays value and captions side-by-side, baseline-aligned (bug-032)', () => {
      const block = ruleBlock('\\.metric-header');
      expect(block).toMatch(/display\s*:\s*flex\b/);
      expect(block).toMatch(/align-items\s*:\s*flex-end\b/);
    });
  });
});
