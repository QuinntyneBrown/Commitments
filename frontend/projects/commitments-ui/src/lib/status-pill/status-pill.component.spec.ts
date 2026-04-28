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

  describe('optional dot (bug-083)', () => {
    it('defaults dot to true', () => {
      const component = TestBed.createComponent(StatusPillComponent).componentInstance;
      expect(component.dot()).toBe(true);
    });

    it('renders the dot span by default', () => {
      const fixture = TestBed.createComponent(StatusPillComponent);
      fixture.detectChanges();
      const dot = (fixture.nativeElement as HTMLElement).querySelector('.status-pill__dot');
      expect(dot).not.toBeNull();
    });

    it('omits the dot span when [dot]=false', () => {
      const fixture = TestBed.createComponent(StatusPillComponent);
      fixture.componentRef.setInput('dot', false);
      fixture.detectChanges();
      const dot = (fixture.nativeElement as HTMLElement).querySelector('.status-pill__dot');
      expect(dot).toBeNull();
    });
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
      // Tolerant of either `var(--cui-info)` (literal) or
      // `@include tinted-pill(--cui-info, …)` (post bug-062).
      expect(block).toMatch(/--cui-info\b/);
    });

    it('exposes a success variant tinted with --cui-success (bug-033)', () => {
      const block = ruleBlock('\\.status-pill--success');
      expect(block).toMatch(/--cui-success\b/);
    });

    it('exposes a warning variant tinted with --cui-warning (bug-035)', () => {
      const block = ruleBlock('\\.status-pill--warning');
      expect(block).toMatch(/--cui-warning\b/);
    });

    it('declares a tinted-pill mixin that variants share (bug-062)', () => {
      expect(scss).toMatch(/@mixin\s+tinted-pill\b/);
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

  describe('template (bug-069)', () => {
    const html = readFileSync(
      join(__dirname, 'status-pill.component.html'),
      'utf8'
    );

    it('drops redundant static class alongside dynamic [class]', () => {
      // The dynamic binding already emits the base `status-pill` class.
      expect(html).not.toMatch(/<mat-chip\s+class="status-pill"\s+\[class\]/);
    });
  });
});
