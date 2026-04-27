// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes } from '@angular/router';
import { DashboardShellComponent } from '@commitments/dashboard-framework';

import { DashboardLayoutComponent } from './components/dashboard-layout/dashboard-layout.component';
import { LoginPageComponent } from './pages/login/login-page/login-page.component';

export const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  {
    path: '',
    component: DashboardLayoutComponent,
    children: [
      { path: '', component: DashboardShellComponent },
      {
        path: 'activities',
        loadComponent: () =>
          import('./pages/activities/activities-page/activities-page.component').then(m => m.ActivitiesPageComponent)
      },
      {
        path: 'behaviours',
        loadComponent: () =>
          import('./pages/behaviours/behaviours-page/behaviours-page.component').then(m => m.BehavioursPageComponent)
      },
      {
        path: 'behaviour-types',
        loadComponent: () =>
          import('./pages/behaviour-types/behaviour-types-page/behaviour-types-page.component').then(m => m.BehaviourTypesPageComponent)
      },
      {
        path: 'commitments',
        loadComponent: () =>
          import('./pages/commitments/commitments-page/commitments-page.component').then(m => m.CommitmentsPageComponent)
      },
      {
        path: 'cards',
        loadComponent: () =>
          import('./pages/cards/cards-page/cards-page.component').then(m => m.CardsPageComponent)
      },
      {
        path: 'card-layouts',
        loadComponent: () =>
          import('./pages/card-layouts/card-layouts-page/card-layouts-page.component').then(m => m.CardLayoutsPageComponent)
      },
      {
        path: 'frequencies',
        loadComponent: () =>
          import('./pages/frequencies/frequencies-page/frequencies-page.component').then(m => m.FrequenciesPageComponent)
      },
      {
        path: 'notes',
        loadComponent: () =>
          import('./pages/notes/notes-page/notes-page.component').then(m => m.NotesPageComponent)
      },
      {
        path: 'profiles',
        loadComponent: () =>
          import('./pages/profiles/profiles-page/profiles-page.component').then(m => m.ProfilesPageComponent)
      },
      {
        path: 'to-dos',
        loadComponent: () =>
          import('./pages/to-dos/to-dos-page/to-dos-page.component').then(m => m.ToDosPageComponent)
      },
      {
        path: 'my-profile',
        loadComponent: () =>
          import('./pages/my-profile/my-profile-page/my-profile-page.component').then(m => m.MyProfilePageComponent)
      },
      {
        path: 'settings',
        loadComponent: () =>
          import('./pages/settings/settings-page/settings-page.component').then(m => m.SettingsPageComponent)
      }
    ]
  }
];
