import { IconButtonComponent } from './icon-button.component';

describe('IconButtonComponent', () => {
  it('exposes icon and ariaLabel inputs', () => {
    const component = new IconButtonComponent();
    component.icon = 'play_arrow';
    component.ariaLabel = 'Play';

    expect(component.icon).toBe('play_arrow');
    expect(component.ariaLabel).toBe('Play');
  });

  it('toggles pressed state when toggle is invoked', () => {
    const component = new IconButtonComponent();
    component.pressed = false;
    let emitted: boolean | null = null;
    component.pressedChange.subscribe(value => {
      emitted = value;
    });

    component.toggle();

    expect(component.pressed).toBe(true);
    expect(emitted).toBe(true);
  });

  it('does not toggle when disabled', () => {
    const component = new IconButtonComponent();
    component.pressed = false;
    component.disabled = true;
    let emitCount = 0;
    component.pressedChange.subscribe(() => {
      emitCount++;
    });

    component.toggle();

    expect(component.pressed).toBe(false);
    expect(emitCount).toBe(0);
  });
});
