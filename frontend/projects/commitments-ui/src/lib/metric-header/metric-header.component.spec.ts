import { TestBed } from '@angular/core/testing';

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
});
