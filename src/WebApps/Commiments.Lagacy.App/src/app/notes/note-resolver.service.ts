// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { RouterStateSnapshot } from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';
import { map, catchError, tap } from 'rxjs';
import { Store } from '../core/store';
import { Injectable } from '@angular/core';
import { Note } from './note';
import { NotesService } from './notes.service';

@Injectable()
export class NoteResolver implements Resolve<Note> {
  constructor(private readonly _notesService: NotesService, private readonly _store: Store) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Note> {
    const slug = route.params['slug'];

    if (!slug) {
      const note = new Note();
      this._store.note$.next(note);
      return of(note);
    }

    return this._notesService
      .getBySlug({ slug })
      .pipe(tap(x => this._store.note$.next(x.note)), map(x => x.note));
  }
}

