import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { ProfilesPageComponent } from './pages/profiles-page/profiles-page.component';
import { MyProfilePageComponent } from './pages/my-profile-page/my-profile-page.component';

export const identityRoutes: Routes = [
  { path: 'login',      component: LoginPageComponent },
  { path: 'profiles',   component: ProfilesPageComponent },
  { path: 'my-profile', component: MyProfilePageComponent },
];
