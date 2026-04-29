import { Routes } from '@angular/router';
import { ProfilesPageComponent } from './pages/profiles-page/profiles-page.component';
import { MyProfilePageComponent } from './pages/my-profile-page/my-profile-page.component';

export const identityRoutes: Routes = [
  { path: 'profiles',   component: ProfilesPageComponent },
  { path: 'my-profile', component: MyProfilePageComponent },
];
