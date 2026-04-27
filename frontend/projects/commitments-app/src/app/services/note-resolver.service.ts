// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { of } from 'rxjs';
import { map, tap } from 'rxjs';
import { Store } from '../core/store';
import { Note } from '../models/note';
import { NotesService } from './notes.service';

export const noteResolver: ResolveFn<Note> = (route, state) => {
  const notesService = inject(NotesService);
  const store = inject(Store);
  const slug = route.params['slug'];

  if (!slug) {
    const note = new Note();
    store.note$.next(note);
    return of(note);
  }

  return notesService
    .getBySlug({ slug })
    .pipe(tap(x => store.note$.next(x.note)), map(x => x.note));
};
