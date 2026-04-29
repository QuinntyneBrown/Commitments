import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { NotesService } from './notes.service';
import { Note } from './note';

export const noteResolver: ResolveFn<Note> = (route) => {
  const service = inject(NotesService);
  const slug = route.paramMap.get('slug')!;
  return service.getBySlug(slug).then(r => r.note);
};
