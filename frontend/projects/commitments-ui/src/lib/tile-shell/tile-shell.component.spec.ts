import { TestBed } from '@angular/core/testing';

import { TileShellComponent } from './tile-shell.component';

describe('TileShellComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [TileShellComponent] });
  });

  it('defaults to the metric variant', () => {
    const component = TestBed.createComponent(TileShellComponent).componentInstance;
    expect(component.variant()).toBe('metric');
  });
});
