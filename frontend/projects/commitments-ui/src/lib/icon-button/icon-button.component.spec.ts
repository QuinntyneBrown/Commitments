import { TestBed } from '@angular/core/testing';

import { IconButtonComponent } from './icon-button.component';

describe('IconButtonComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [IconButtonComponent] });
  });

  it('exposes icon and ariaLabel inputs', () => {
    const component = TestBed.createComponent(IconButtonComponent).componentInstance;

    expect(component.icon()).toBe('');
    expect(component.ariaLabel()).toBe('');
  });

  it('toggles pressed state when toggle is invoked', () => {
    const component = TestBed.createComponent(IconButtonComponent).componentInstance;
    component.pressed.set(false);

    component.toggle();

    expect(component.pressed()).toBe(true);
  });

  it('does not toggle when disabled', () => {
    const fixture = TestBed.createComponent(IconButtonComponent);
    fixture.componentRef.setInput('disabled', true);
    const component = fixture.componentInstance;
    component.pressed.set(false);

    component.toggle();

    expect(component.pressed()).toBe(false);
  });
});
