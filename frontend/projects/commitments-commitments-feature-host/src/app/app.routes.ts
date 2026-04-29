import { Routes } from '@angular/router';
import { commitmentsRoutes } from '@commitments/commitments-feature';
export const routes: Routes = [
  { path: '', redirectTo: 'commitments', pathMatch: 'full' },
  ...commitmentsRoutes,
];
