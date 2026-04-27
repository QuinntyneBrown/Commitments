import { Component, runInInjectionContext, signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { Subject } from 'rxjs';

import { bindTileMode, BindTileModeOptions } from './bind-tile-mode';
import { DashboardMode, TileContext } from './tile.model';

function makeContext(overrides?: Partial<TileContext>): TileContext {
  const refresh$ = new Subject<void>();
  return {
    tileId: 't',
    instanceId: 'i',
    isEditMode: signal(false).asReadonly(),
    isMaximized: signal(false).asReadonly(),
    mode: signal<DashboardMode>('live').asReadonly(),
    selectedReviewDate: signal<string | null>(null).asReadonly(),
    refresh$: refresh$.asObservable(),
    requestRefresh: () => refresh$.next(),
    remove: () => {},
    maximize: () => {},
    restore: () => {},
    requestFocus: () => {},
    ...overrides
  };
}

@Component({ standalone: true, template: '' })
class HostComponent {}

function mount(opts: BindTileModeOptions) {
  const fixture = TestBed.createComponent(HostComponent);
  runInInjectionContext(fixture.componentRef.injector, () => bindTileMode(opts));
  fixture.detectChanges();
  return fixture;
}

describe('bindTileMode', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({ imports: [HostComponent] });
  });

  it('calls load(mode, asOf) once on first invocation', () => {
    const load = jest.fn();
    mount({ context: makeContext(), load });

    expect(load).toHaveBeenCalledTimes(1);
    expect(load).toHaveBeenCalledWith('live', null);
  });

  it('treats null context as live mode with no review date', () => {
    const load = jest.fn();
    mount({ context: null, load });

    expect(load).toHaveBeenCalledTimes(1);
    expect(load).toHaveBeenCalledWith('live', null);
  });

  it('calls load when context.mode() changes', () => {
    const load = jest.fn();
    const mode = signal<DashboardMode>('live');
    const fixture = mount({ context: makeContext({ mode: mode.asReadonly() }), load });
    load.mockClear();

    mode.set('review');
    fixture.detectChanges();

    expect(load).toHaveBeenCalledWith('review', null);
  });

  it('calls load when context.selectedReviewDate() changes', () => {
    const load = jest.fn();
    const date = signal<string | null>(null);
    const fixture = mount({
      context: makeContext({ selectedReviewDate: date.asReadonly() }),
      load
    });
    load.mockClear();

    date.set('2026-04-15T18:30:00Z');
    fixture.detectChanges();

    expect(load).toHaveBeenCalledWith('live', '2026-04-15T18:30:00Z');
  });

  it('calls load when refresh$ emits', () => {
    const load = jest.fn();
    const refresh$ = new Subject<void>();
    mount({ context: makeContext({ refresh$: refresh$.asObservable() }), load });
    load.mockClear();

    refresh$.next();

    expect(load).toHaveBeenCalledWith('live', null);
  });

  it('calls load when invalidations$ emits', () => {
    const load = jest.fn();
    const invalidations$ = new Subject<unknown>();
    mount({ context: makeContext(), invalidations$: invalidations$.asObservable(), load });
    load.mockClear();

    invalidations$.next('something');

    expect(load).toHaveBeenCalledWith('live', null);
  });

  it('stops calling load after the host is destroyed', () => {
    const load = jest.fn();
    const refresh$ = new Subject<void>();
    const fixture = mount({
      context: makeContext({ refresh$: refresh$.asObservable() }),
      load
    });
    fixture.destroy();
    load.mockClear();

    refresh$.next();

    expect(load).not.toHaveBeenCalled();
  });
});
