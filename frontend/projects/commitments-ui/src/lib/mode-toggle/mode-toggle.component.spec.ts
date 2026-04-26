import { ModeToggleComponent } from './mode-toggle.component';

describe('ModeToggleComponent', () => {
  it('emits modeChange when select is called with a different mode', () => {
    const component = new ModeToggleComponent();
    component.mode = 'live';
    let emitted: 'live' | 'review' | null = null;
    component.modeChange.subscribe(value => {
      emitted = value;
    });

    component.select('review');

    expect(emitted).toBe('review');
  });

  it('does not emit when select is called with the current mode', () => {
    const component = new ModeToggleComponent();
    component.mode = 'live';
    let emitCount = 0;
    component.modeChange.subscribe(() => {
      emitCount++;
    });

    component.select('live');

    expect(emitCount).toBe(0);
  });

  it('does not emit when disabled', () => {
    const component = new ModeToggleComponent();
    component.mode = 'live';
    component.disabled = true;
    let emitCount = 0;
    component.modeChange.subscribe(() => {
      emitCount++;
    });

    component.select('review');

    expect(emitCount).toBe(0);
  });

  it('switches to review on ArrowRight and to live on ArrowLeft', () => {
    const component = new ModeToggleComponent();
    component.mode = 'live';
    const seen: ('live' | 'review')[] = [];
    component.modeChange.subscribe(value => {
      seen.push(value);
    });

    component.onKeyDown(new KeyboardEvent('keydown', { key: 'ArrowRight' }));
    component.onKeyDown(new KeyboardEvent('keydown', { key: 'ArrowLeft' }));

    expect(seen).toEqual(['review', 'live']);
  });
});
