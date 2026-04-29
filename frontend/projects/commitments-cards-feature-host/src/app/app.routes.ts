import { Routes } from '@angular/router';
import { cardsRoutes } from '@commitments/cards-feature';
export const routes: Routes = [
  { path: '', redirectTo: 'cards', pathMatch: 'full' },
  ...cardsRoutes,
];
