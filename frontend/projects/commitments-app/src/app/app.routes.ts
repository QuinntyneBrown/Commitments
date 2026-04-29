// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes } from '@angular/router';
import { DashboardShellComponent } from '@commitments/dashboard-framework';

import { DashboardLayoutComponent } from './components/dashboard-layout/dashboard-layout.component';
import { ActivitiesPageComponent } from './pages/activities/activities-page/activities-page.component';
import { ToDosPageComponent } from './pages/to-dos/to-dos-page/to-dos-page.component';
import { CardsPageComponent } from './pages/cards/cards-page/cards-page.component';
import { CardLayoutsPageComponent } from './pages/card-layouts/card-layouts-page/card-layouts-page.component';
import { behavioursRoutes } from '@commitments/behaviours-feature';
import { commitmentsRoutes } from '@commitments/commitments-feature';
import { frequenciesRoutes } from '@commitments/frequencies-feature';
import { notesRoutes } from '@commitments/notes-feature';
import { LoginPageComponent } from './pages/login/login-page/login-page.component';
import { MyProfilePageComponent } from './pages/my-profile/my-profile-page/my-profile-page.component';
import { ProfilesPageComponent } from './pages/profiles/profiles-page/profiles-page.component';
import { SettingsPageComponent } from './pages/settings/settings-page/settings-page.component';

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
      ...behavioursRoutes,
      ...frequenciesRoutes,
      ...commitmentsRoutes,
      { path: 'activities', component: ActivitiesPageComponent },
      { path: 'to-dos', component: ToDosPageComponent },
      ...notesRoutes,
      { path: 'cards', component: CardsPageComponent },
      { path: 'card-layouts', component: CardLayoutsPageComponent }
    ]
  }
];
