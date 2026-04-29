import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Note } from './note';
import { NotesService } from './notes.service';

export const noteResolver: ResolveFn<Note> = (route) => {
  const slug = route.paramMap.get('slug') ?? '';
  return inject(NotesService).getBySlug(slug).then(r => r.note);
};
