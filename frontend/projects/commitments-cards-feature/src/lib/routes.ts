import { Routes } from '@angular/router';
import { CardsPageComponent } from './pages/cards-page/cards-page.component';
import { CardLayoutsPageComponent } from './pages/card-layouts-page/card-layouts-page.component';

export const cardsRoutes: Routes = [
  { path: 'cards',        component: CardsPageComponent },
  { path: 'card-layouts', component: CardLayoutsPageComponent },
];
