import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { readFileSync } from 'fs';
import { join } from 'path';

import { TileShellComponent } from './tile-shell.component';

@Component({
  standalone: true,
  imports: [TileShellComponent],
  template: `
    <commitments-tile-shell title="Demo" eyebrow="Today" icon="today">
      <span tile-status data-testid="projected-status">LIVE</span>
      <p data-testid="projected-body">body</p>
    </commitments-tile-shell>
  `
})
class TileShellHostComponent {}

describe('TileShellComponent', () => {
  it('defaults to the metric variant', () => {
    TestBed.configureTestingModule({ imports: [TileShellComponent] });
    const component = TestBed.createComponent(TileShellComponent).componentInstance;
    expect(component.variant()).toBe('metric');
  });

  it('no longer exposes the legacy `status` input (bug-059)', () => {
    const component = TestBed.createComponent(TileShellComponent).componentInstance as unknown as Record<string, unknown>;
    expect(component['status']).toBeUndefined();
  });

  describe('named projection', () => {
    function render() {
      TestBed.configureTestingModule({ imports: [TileShellHostComponent] });
      const fixture = TestBed.createComponent(TileShellHostComponent);
      fixture.detectChanges();
      return fixture.nativeElement as HTMLElement;
    }

    it('projects [tile-status] into the header, not the body', () => {
      const root = render();
      const header = root.querySelector('mat-card-header')!;
      const body = root.querySelector('mat-card-content')!;
      const status = root.querySelector('[data-testid="projected-status"]')!;
      expect(header.contains(status)).toBe(true);
      expect(body.contains(status)).toBe(false);
    });

    it('still projects unslotted content into the body', () => {
      const root = render();
      const body = root.querySelector('mat-card-content')!;
      const projectedBody = root.querySelector('[data-testid="projected-body"]')!;
      expect(body.contains(projectedBody)).toBe(true);
    });
  });

  describe('eyebrow placement', () => {
    function render() {
      TestBed.configureTestingModule({ imports: [TileShellHostComponent] });
      const fixture = TestBed.createComponent(TileShellHostComponent);
      fixture.detectChanges();
      return fixture.nativeElement as HTMLElement;
    }

    it('renders the eyebrow after the title-row in DOM order', () => {
      const root = render();
      const titleRow = root.querySelector('.tile-shell__title-row')!;
      const eyebrow = root.querySelector('.tile-shell__eyebrow')!;

      expect(titleRow).toBeTruthy();
      expect(eyebrow).toBeTruthy();

      const order = titleRow.compareDocumentPosition(eyebrow);
      expect(order & Node.DOCUMENT_POSITION_FOLLOWING).toBeTruthy();
    });
  });

  describe('header chrome (CSS source)', () => {
    const scss = readFileSync(join(__dirname, 'tile-shell.component.scss'), 'utf8');

    function ruleBlock(selector: string): string {
      const match = scss.match(new RegExp(`${selector}\\s*\\{([^}]*)\\}`));
      return match ? match[1] : '';
    }

    it('does not draw a divider beneath the header (bug-020)', () => {
      const block = ruleBlock('\\.tile-shell__header');
      expect(block).not.toMatch(/border-bottom\s*:/);
    });

    it('renders the title at 16px to match every tile design (bug-034)', () => {
      const block = ruleBlock('\\.tile-shell__title');
      expect(block).toMatch(/font-size\s*:\s*16px\b/);
    });

    it('body padding-top consumes the per-tile custom property (bug-041)', () => {
      const block = ruleBlock('\\.tile-shell__body');
      expect(block).toMatch(/padding-top\s*:\s*var\(\s*--cui-tile-body-gap/);
    });

    it('drops the chart-variant body padding override after bug-041 refactor', () => {
      // bug-040 introduced `.tile-shell--chart .tile-shell__body { padding-top: 14px }`;
      // bug-041 replaces it with the per-tile custom property.
      expect(scss).not.toMatch(/\.tile-shell--chart\s+\.tile-shell__body\s*\{[^}]*padding-top/);
    });

    it('renders the .pen drop shadow on every tile (bug-042)', () => {
      const block = ruleBlock('\\.tile-shell');
      expect(block).toMatch(/box-shadow\s*:[^;]*4px[^;]*12px/);
      expect(block).not.toMatch(/box-shadow\s*:\s*none\b/);
    });

    it('drops the dead `__status` rule after the named-slot migration (bug-059)', () => {
      expect(scss).not.toMatch(/\.tile-shell__status\b/);
    });

    it('icon colour consumes the per-tile custom property (bug-048)', () => {
      const block = ruleBlock('\\.tile-shell__icon');
      expect(block).toMatch(/color\s*:\s*var\(\s*--cui-tile-icon-color/);
    });

    it('icon size consumes the per-tile custom property (bug-049)', () => {
      const block = ruleBlock('\\.tile-shell__icon');
      expect(block).toMatch(/font-size\s*:\s*var\(\s*--cui-tile-icon-size/);
      expect(block).toMatch(/width\s*:\s*var\(\s*--cui-tile-icon-size/);
      expect(block).toMatch(/height\s*:\s*var\(\s*--cui-tile-icon-size/);
    });

    it('title-row gap consumes the per-tile custom property (bug-050)', () => {
      const block = ruleBlock('\\.tile-shell__title-row');
      expect(block).toMatch(/gap\s*:\s*var\(\s*--cui-tile-header-gap/);
    });
  });
});
