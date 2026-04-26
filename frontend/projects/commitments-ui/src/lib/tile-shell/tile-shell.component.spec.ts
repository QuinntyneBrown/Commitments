import { TileShellComponent } from './tile-shell.component';

describe('TileShellComponent', () => {
  it('defaults to the metric variant', () => {
    const component = new TileShellComponent();
    expect(component.variant).toBe('metric');
  });

  it('accepts metric, review, and chart variants', () => {
    const component = new TileShellComponent();
    component.variant = 'review';
    expect(component.variant).toBe('review');

    component.variant = 'chart';
    expect(component.variant).toBe('chart');
  });
});
