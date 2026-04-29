import { Routes } from '@angular/router';
import { ActivitiesPageComponent } from './pages/activities-page/activities-page.component';
import { ToDosPageComponent } from './pages/to-dos-page/to-dos-page.component';

export const trackingRoutes: Routes = [
  { path: 'activities', component: ActivitiesPageComponent },
  { path: 'to-dos',     component: ToDosPageComponent },
];
