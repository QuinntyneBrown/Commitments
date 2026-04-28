import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';

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

    it('renders the eyebrow after the title-row, in sentence case', () => {
      const root = render();
      const titleRow = root.querySelector('.tile-shell__title-row')!;
      const eyebrow = root.querySelector('.tile-shell__eyebrow')!;

      expect(eyebrow).toBeTruthy();
      expect(titleRow).toBeTruthy();

      const order = titleRow.compareDocumentPosition(eyebrow);
      expect(order & Node.DOCUMENT_POSITION_FOLLOWING).toBeTruthy();

      const style = getComputedStyle(eyebrow);
      expect(style.textTransform).toBe('none');
      expect(parseInt(style.fontWeight, 10)).toBeLessThanOrEqual(500);
    });
  });
});
