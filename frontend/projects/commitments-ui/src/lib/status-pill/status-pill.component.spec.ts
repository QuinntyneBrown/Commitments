import { TestBed } from '@angular/core/testing';
import { readFileSync } from 'fs';
import { join } from 'path';

import { StatusPillComponent } from './status-pill.component';

describe('StatusPillComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [StatusPillComponent] });
  });

  it('defaults to neutral variant and no pulse', () => {
    const component = TestBed.createComponent(StatusPillComponent).componentInstance;
    expect(component.variant()).toBe('neutral');
    expect(component.pulse()).toBe(false);
    expect(component.label()).toBe('');
  });

  describe('CSS source', () => {
    const scss = readFileSync(
      join(__dirname, 'status-pill.component.scss'),
      'utf8'
    );

    function ruleBlock(selector: string): string {
      const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
      return match ? match[1] : '';
    }

    it('exposes a chart variant tinted with --cui-info (bug-031)', () => {
      const block = ruleBlock('\\.status-pill--chart');
      expect(block).toMatch(/var\(--cui-info/);
    });

    it('exposes a success variant tinted with --cui-success (bug-033)', () => {
      const block = ruleBlock('\\.status-pill--success');
      expect(block).toMatch(/var\(--cui-success/);
    });

    it('renders the dot at 8x8 (bug-034)', () => {
      const block = ruleBlock('\\.status-pill__dot');
      expect(block).toMatch(/width\s*:\s*8px\b/);
      expect(block).toMatch(/height\s*:\s*8px\b/);
    });

    it('uses 1px letter-spacing on the pill text (bug-034)', () => {
      const block = ruleBlock('\\.status-pill');
      expect(block).toMatch(/letter-spacing\s*:\s*1px\b/);
    });
  });
});
