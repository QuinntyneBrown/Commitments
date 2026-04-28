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
  });
});
