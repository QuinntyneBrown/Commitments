import { Injectable, inject } from '@angular/core';
import { DashboardBackendService } from '@commitments/dashboard-framework';
import { Note } from './note';

@Injectable({ providedIn: 'root' })
export class NotesService {
  private readonly _backend = inject(DashboardBackendService);
  list(): Promise<{ notes: Note[] }>                           { return this._backend.get('api/v1.0/notes'); }
  getBySlug(slug: string): Promise<{ note: Note }>             { return this._backend.get(`api/v1.0/notes/slug/${slug}`); }
  getByTagSlug(slug: string): Promise<{ notes: Note[] }>       { return this._backend.get(`api/v1.0/notes/tag/${slug}`); }
  save(input: { note: Partial<Note> }): Promise<{ note: Note }> { return this._backend.post('api/v1.0/notes', input); }
  addTag(noteId: number, tagId: number): Promise<void>         { return this._backend.post(`api/v1.0/notes/${noteId}/tag/${tagId}`, {}); }
  removeTag(noteId: number, tagId: number): Promise<void>      { return this._backend.post(`api/v1.0/notes/${noteId}/removeTag`, { tagId }); }
  remove(id: number | string): Promise<void>                   { return this._backend.delete(`api/v1.0/notes/${id}`); }
}
