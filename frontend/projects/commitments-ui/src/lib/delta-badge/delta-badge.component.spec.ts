import { DeltaBadgeComponent } from './delta-badge.component';

describe('DeltaBadgeComponent', () => {
  it('formats positive deltas with a leading + and percent sign in percent mode', () => {
    const component = new DeltaBadgeComponent();
    component.delta = 12;
    component.format = 'percent';

    expect(component.formatted()).toBe('+12%');
    expect(component.tone()).toBe('positive');
  });

  it('formats negative deltas with a leading - in count mode', () => {
    const component = new DeltaBadgeComponent();
    component.delta = -5;
    component.format = 'count';

    expect(component.formatted()).toBe('-5');
    expect(component.tone()).toBe('negative');
  });

  it('formats zero with the equals symbol and a neutral tone', () => {
    const component = new DeltaBadgeComponent();
    component.delta = 0;
    component.format = 'percent';

    expect(component.formatted()).toBe('±0%');
    expect(component.tone()).toBe('neutral');
  });
});
