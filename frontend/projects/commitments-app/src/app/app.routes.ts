// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes } from '@angular/router';
import { DashboardShellComponent } from '@commitments/dashboard-framework';

import { DashboardLayoutComponent } from './components/dashboard-layout/dashboard-layout.component';
import { PlaceholderPageComponent } from './components/placeholder-page/placeholder-page.component';
import { BehaviourTypesPageComponent } from './pages/behaviour-types/behaviour-types-page/behaviour-types-page.component';
import { LoginPageComponent } from './pages/login/login-page/login-page.component';
import { MyProfilePageComponent } from './pages/my-profile/my-profile-page/my-profile-page.component';
import { ProfilesPageComponent } from './pages/profiles/profiles-page/profiles-page.component';
import { SettingsPageComponent } from './pages/settings/settings-page/settings-page.component';

const placeholderPaths = [
  'activities',
  'behaviours',
  'commitments',
  'cards',
  'card-layouts',
  'frequencies',
  'notes',
  'to-dos'
];

export const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  {
    path: '',
    component: DashboardLayoutComponent,
    children: [
      { path: '', component: DashboardShellComponent },
      { path: 'profiles', component: ProfilesPageComponent },
      { path: 'my-profile', component: MyProfilePageComponent },
      { path: 'settings', component: SettingsPageComponent },
      { path: 'behaviour-types', component: BehaviourTypesPageComponent },
      ...placeholderPaths.map(path => ({ path, component: PlaceholderPageComponent }))
    ]
  }
];
