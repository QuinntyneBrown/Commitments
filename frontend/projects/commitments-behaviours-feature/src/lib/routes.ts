import { Routes } from '@angular/router';
import { BehaviourTypesPageComponent } from './pages/behaviour-types-page/behaviour-types-page.component';
import { BehavioursPageComponent } from './pages/behaviours-page/behaviours-page.component';

export const behavioursRoutes: Routes = [
  { path: 'behaviour-types', component: BehaviourTypesPageComponent },
  { path: 'behaviours',      component: BehavioursPageComponent },
];
