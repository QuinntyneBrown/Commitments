import { Routes } from '@angular/router';
import { identityRoutes } from '@commitments/identity-feature';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  ...identityRoutes,
];
