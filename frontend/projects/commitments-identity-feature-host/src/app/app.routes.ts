import { Routes } from '@angular/router';
import { LoginPageComponent, identityRoutes } from '@commitments/identity-feature';

export const routes: Routes = [
  { path: '', redirectTo: 'profiles', pathMatch: 'full' },
  { path: 'login', component: LoginPageComponent },
  ...identityRoutes,
];
