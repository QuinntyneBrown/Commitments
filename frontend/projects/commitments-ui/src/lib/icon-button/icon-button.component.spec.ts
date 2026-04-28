import { TestBed } from '@angular/core/testing';
import { readFileSync } from 'fs';
import { join } from 'path';

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

  it('every var(--cui-*) reference in SCSS includes a fallback hex (bug-104)', () => {
    const scss = readFileSync(
      join(__dirname, 'icon-button.component.scss'),
      'utf8'
    );
    const offenders = scss
      .split(/\r?\n/)
      .map((line, i) => ({ n: i + 1, line }))
      .filter(({ line }) => /var\(--cui-[a-z0-9-]+\)/.test(line));
    expect(offenders).toEqual([]);
  });
});
