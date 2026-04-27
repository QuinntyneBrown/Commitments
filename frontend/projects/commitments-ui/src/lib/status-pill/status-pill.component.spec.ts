import { TestBed } from '@angular/core/testing';

import { StatusPillComponent } from './status-pill.component';

describe('StatusPillComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [StatusPillComponent] });
  });

  it('defaults to neutral variant and no pulse', () => {
    const component = TestBed.createComponent(StatusPillComponent).componentInstance;
    expect(component.variant()).toBe('neutral');
    expect(component.pulse()).toBe(false);
    expect(component.label()).toBe('');
  });
});
