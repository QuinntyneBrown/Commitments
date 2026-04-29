import { Routes } from '@angular/router';
import { settingsRoutes } from '@commitments/settings-feature';
export const routes: Routes = [
  { path: '', redirectTo: 'settings', pathMatch: 'full' },
  ...settingsRoutes,
];
