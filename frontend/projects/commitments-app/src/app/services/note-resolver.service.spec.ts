// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { TestBed } from '@angular/core/testing';
import { runInInjectionContext } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { firstValueFrom, of } from 'rxjs';

import { noteResolver } from './note-resolver.service';
import { NotesService } from './notes.service';
import { Store } from '../core/store';

describe('noteResolver', () => {
  function setup(slug: string | undefined, getBySlug = jest.fn()) {
    const store = { note: Object.assign(jest.fn(() => ({})), { set: jest.fn(), update: jest.fn() }) } as unknown as Store;
    TestBed.configureTestingModule({
      providers: [
        { provide: NotesService, useValue: { getBySlug } },
        { provide: Store, useValue: store }
      ]
    });
    const route = { params: { slug } } as unknown as ActivatedRouteSnapshot;
    const state = {} as RouterStateSnapshot;
    return { run: () => runInInjectionContext(TestBed.inject(TestBed['_injector']), () => noteResolver(route, state)), getBySlug, store };
  }

  it('returns an empty Note without HTTP when slug is "new" (design 13 Slice D)', async () => {
    const getBySlug = jest.fn();
    const store = { note: Object.assign(jest.fn(() => ({})), { set: jest.fn(), update: jest.fn() }) } as unknown as Store;
    TestBed.configureTestingModule({
      providers: [
        { provide: NotesService, useValue: { getBySlug } },
        { provide: Store, useValue: store }
      ]
    });

    const route = { params: { slug: 'new' } } as unknown as ActivatedRouteSnapshot;
    const state = {} as RouterStateSnapshot;

    const result = await TestBed.runInInjectionContext(() => firstValueFrom(noteResolver(route, state) as any));

    expect(getBySlug).not.toHaveBeenCalled();
    expect(result).toBeDefined();
  });

  it('fetches by slug and pushes to the store for non-"new" slugs', async () => {
    const fetched = { note: { noteId: 'abc', title: 'T', slug: 'the-slug', body: 'x' } };
    const getBySlug = jest.fn().mockReturnValue(of(fetched));
    const store = { note: Object.assign(jest.fn(() => ({})), { set: jest.fn(), update: jest.fn() }) } as unknown as Store;
    TestBed.configureTestingModule({
      providers: [
        { provide: NotesService, useValue: { getBySlug } },
        { provide: Store, useValue: store }
      ]
    });

    const route = { params: { slug: 'the-slug' } } as unknown as ActivatedRouteSnapshot;
    const state = {} as RouterStateSnapshot;

    await TestBed.runInInjectionContext(() => firstValueFrom(noteResolver(route, state) as any));

    expect(getBySlug).toHaveBeenCalledWith({ slug: 'the-slug' });
    expect((store.note as any).set).toHaveBeenCalledWith(fetched.note);
  });
});
