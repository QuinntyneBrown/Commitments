import { Routes } from '@angular/router';
import { frequenciesRoutes } from '@commitments/frequencies-feature';

export const routes: Routes = [
  { path: '', redirectTo: 'frequencies', pathMatch: 'full' },
  ...frequenciesRoutes,
];
