import { Routes } from '@angular/router';
import { NotesPageComponent } from './pages/notes-page/notes-page.component';
import { EditNotePageComponent } from './pages/edit-note-page/edit-note-page.component';
import { TagsPageComponent } from './pages/tags-page/tags-page.component';
import { NotesByTagPageComponent } from './pages/notes-by-tag-page/notes-by-tag-page.component';
import { noteResolver } from './data/note-resolver.service';

export const notesRoutes: Routes = [
  { path: 'notes',                 component: NotesPageComponent },
  { path: 'edit-note/:slug',       component: EditNotePageComponent, resolve: { note: noteResolver } },
  { path: 'tags',                  component: TagsPageComponent },
  { path: 'notes-by-tag/:slug',    component: NotesByTagPageComponent },
];
