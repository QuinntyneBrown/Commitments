import { Routes } from '@angular/router';
import { notesRoutes } from '@commitments/notes-feature';

export const routes: Routes = [
  { path: '', redirectTo: 'notes', pathMatch: 'full' },
  ...notesRoutes,
];
