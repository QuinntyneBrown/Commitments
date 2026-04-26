import { MetricHeaderComponent } from './metric-header.component';

describe('MetricHeaderComponent', () => {
  it('defaults to chart accent and empty caption', () => {
    const component = new MetricHeaderComponent();
    expect(component.value).toBe('');
    expect(component.caption).toBe('');
    expect(component.accent).toBe('chart');
  });

  it('accepts live, review, and chart accents', () => {
    const component = new MetricHeaderComponent();
    component.accent = 'live';
    expect(component.accent).toBe('live');
    component.accent = 'review';
    expect(component.accent).toBe('review');
  });
});
