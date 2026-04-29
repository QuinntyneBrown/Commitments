import { Routes } from '@angular/router';
import { trackingRoutes } from '@commitments/tracking-feature';
export const routes: Routes = [
  { path: '', redirectTo: 'activities', pathMatch: 'full' },
  ...trackingRoutes,
];
