import { TestBed } from '@angular/core/testing';

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
});
