import { Routes } from '@angular/router';
import { behavioursRoutes } from '@commitments/behaviours-feature';

export const routes: Routes = [
  { path: '', redirectTo: 'behaviours', pathMatch: 'full' },
  ...behavioursRoutes,
];
