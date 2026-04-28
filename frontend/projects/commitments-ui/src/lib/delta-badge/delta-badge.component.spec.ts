import { TestBed } from '@angular/core/testing';
import { readFileSync } from 'fs';
import { join } from 'path';

import { DeltaBadgeComponent } from './delta-badge.component';

describe('DeltaBadgeComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [DeltaBadgeComponent] });
  });

  it('formats positive deltas with a leading + and percent sign in percent mode', () => {
    const fixture = TestBed.createComponent(DeltaBadgeComponent);
    fixture.componentRef.setInput('delta', 12);
    fixture.componentRef.setInput('format', 'percent');
    const component = fixture.componentInstance;

    expect(component.formatted()).toBe('+12%');
    expect(component.tone()).toBe('positive');
  });

  it('formats negative deltas with a leading - in count mode', () => {
    const fixture = TestBed.createComponent(DeltaBadgeComponent);
    fixture.componentRef.setInput('delta', -5);
    fixture.componentRef.setInput('format', 'count');
    const component = fixture.componentInstance;

    expect(component.formatted()).toBe('-5');
    expect(component.tone()).toBe('negative');
  });

  it('formats zero with the equals symbol and a neutral tone', () => {
    const component = TestBed.createComponent(DeltaBadgeComponent).componentInstance;

    expect(component.formatted()).toBe('±0%');
    expect(component.tone()).toBe('neutral');
  });

  describe('CSS source', () => {
    const scss = readFileSync(
      join(__dirname, 'delta-badge.component.scss'),
      'utf8'
    );

    function ruleBlock(selector: string): string {
      const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
      return match ? match[1] : '';
    }

    it('renders as a 20px rounded rect with .pen-aligned padding/gap (bug-052)', () => {
      const block = ruleBlock('\\.delta-badge');
      expect(block).toMatch(/border-radius\s*:\s*4px\b/);
      expect(block).toMatch(/min-height\s*:\s*20px\b/);
      expect(block).toMatch(/padding\s*:\s*0\s+6px\b/);
      expect(block).toMatch(/gap\s*:\s*4px\b/);
    });

    it('aligns icon and text to vertical center, matching ctDelta (bug-086)', () => {
      const block = ruleBlock('\\.delta-badge');
      expect(block).toMatch(/align-items\s*:\s*center\b/);
      expect(block).not.toMatch(/align-items\s*:\s*baseline\b/);
    });

    it('pins font-family to --cui-font-display at the root (bug-112)', () => {
      const block = ruleBlock('\\.delta-badge');
      expect(block).toMatch(/font-family\s*:\s*var\(\s*--cui-font-display/);
    });

    it('uses computed() signals for formatted / tone / icon (bug-129)', () => {
      const ts = readFileSync(
        join(__dirname, 'delta-badge.component.ts'),
        'utf8'
      );
      expect(ts).toMatch(/import\s*\{[^}]*\bcomputed\b[^}]*\}\s*from\s*'@angular\/core'/);
      expect(ts).toMatch(/\bformatted\s*=\s*computed\(/);
      expect(ts).toMatch(/\btone\s*=\s*computed\b/);
      expect(ts).toMatch(/\bicon\s*=\s*computed\(/);
      // Old method-form declarations should be gone.
      expect(ts).not.toMatch(/^\s*formatted\(\)\s*:/m);
      expect(ts).not.toMatch(/^\s*tone\(\)\s*:/m);
      expect(ts).not.toMatch(/^\s*icon\(\)\s*:/m);
    });

    it('sizes the directional icon to 12px (bug-053)', () => {
      const block = ruleBlock('\\.delta-badge__icon');
      expect(block).toMatch(/font-size\s*:\s*12px\b/);
      expect(block).toMatch(/width\s*:\s*12px\b/);
      expect(block).toMatch(/height\s*:\s*12px\b/);
    });

    it('renders uniformly at 11px / weight 500 across value + caption (bug-054)', () => {
      const block = ruleBlock('\\.delta-badge');
      expect(block).toMatch(/font-size\s*:\s*11px\b/);
      expect(block).toMatch(/font-weight\s*:\s*500\b/);

      // The value rule must not bump weight back up; caption rule
      // must not override the inherited accent colour.
      const valueBlock = ruleBlock('\\.delta-badge__value');
      expect(valueBlock).not.toMatch(/font-weight\s*:\s*[6-9]\d\d\b/);
      const captionBlock = ruleBlock('\\.delta-badge__caption');
      expect(captionBlock).not.toMatch(/color\s*:\s*var\(\s*--cui-text-secondary/);
    });
  });

  describe('directional icon (bug-053)', () => {
    it('returns arrow_upward for positive deltas', () => {
      const fixture = TestBed.createComponent(DeltaBadgeComponent);
      fixture.componentRef.setInput('delta', 12);
      expect(fixture.componentInstance.icon()).toBe('arrow_upward');
    });

    it('returns arrow_downward for negative deltas', () => {
      const fixture = TestBed.createComponent(DeltaBadgeComponent);
      fixture.componentRef.setInput('delta', -3);
      expect(fixture.componentInstance.icon()).toBe('arrow_downward');
    });

    it('returns empty string for zero (no neutral icon)', () => {
      const component = TestBed.createComponent(DeltaBadgeComponent).componentInstance;
      expect(component.icon()).toBe('');
    });
  });

  describe('template (bug-069)', () => {
    const html = readFileSync(
      join(__dirname, 'delta-badge.component.html'),
      'utf8'
    );

    it('drops redundant static class alongside dynamic [class]', () => {
      expect(html).not.toMatch(/<mat-chip\s+class="delta-badge"\s+\[class\]/);
    });
  });
});
