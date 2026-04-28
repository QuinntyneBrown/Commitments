import { TestBed } from '@angular/core/testing';
import { readFileSync } from 'fs';
import { join } from 'path';

import { ModeToggleComponent } from './mode-toggle.component';

describe('ModeToggleComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [ModeToggleComponent] });
  });

  it('emits modeChange when select is called with a different mode', () => {
    const component = TestBed.createComponent(ModeToggleComponent).componentInstance;
    component.mode.set('live');

    component.select('review');

    expect(component.mode()).toBe('review');
  });

  it('does not emit when select is called with the current mode', () => {
    const component = TestBed.createComponent(ModeToggleComponent).componentInstance;
    component.mode.set('live');

    component.select('live');

    expect(component.mode()).toBe('live');
  });

  it('does not emit when disabled', () => {
    const fixture = TestBed.createComponent(ModeToggleComponent);
    fixture.componentRef.setInput('disabled', true);
    const component = fixture.componentInstance;
    component.mode.set('live');

    component.select('review');

    expect(component.mode()).toBe('live');
  });

  it('switches to review on ArrowRight and to live on ArrowLeft', () => {
    const component = TestBed.createComponent(ModeToggleComponent).componentInstance;
    component.mode.set('live');

    component.onKeyDown(new KeyboardEvent('keydown', { key: 'ArrowRight' }));
    component.onKeyDown(new KeyboardEvent('keydown', { key: 'ArrowLeft' }));

    expect(component.mode()).toBe('live');
  });

  it('pins font-family to --cui-font-display at the root (bug-113)', () => {
    const scss = readFileSync(
      join(__dirname, 'mode-toggle.component.scss'),
      'utf8'
    );
    const block = scss.match(/\.mode-toggle\s*\{([^}]*)\}/);
    expect(block).toBeTruthy();
    expect(block![1]).toMatch(/font-family\s*:\s*var\(\s*--cui-font-display/);
  });
});
