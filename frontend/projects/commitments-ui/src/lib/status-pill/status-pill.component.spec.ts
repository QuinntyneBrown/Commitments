import { StatusPillComponent } from './status-pill.component';

describe('StatusPillComponent', () => {
  it('defaults to neutral variant and no pulse', () => {
    const component = new StatusPillComponent();
    expect(component.variant).toBe('neutral');
    expect(component.pulse).toBe(false);
    expect(component.label).toBe('');
  });

  it('accepts live, review, and neutral variants', () => {
    const component = new StatusPillComponent();
    component.variant = 'live';
    component.label = 'LIVE';
    expect(component.variant).toBe('live');

    component.variant = 'review';
    expect(component.variant).toBe('review');
  });
});
