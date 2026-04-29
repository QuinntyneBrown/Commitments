// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Routes } from '@angular/router';
import { DashboardShellComponent } from '@commitments/dashboard-framework';

import { DashboardLayoutComponent } from './components/dashboard-layout/dashboard-layout.component';
import { behavioursRoutes } from '@commitments/behaviours-feature';
import { cardsRoutes } from '@commitments/cards-feature';
import { commitmentsRoutes } from '@commitments/commitments-feature';
import { frequenciesRoutes } from '@commitments/frequencies-feature';
import { authGuard, identityRoutes, LoginPageComponent } from '@commitments/identity-feature';
import { notesRoutes } from '@commitments/notes-feature';
import { settingsRoutes } from '@commitments/settings-feature';
import { trackingRoutes } from '@commitments/tracking-feature';

export const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  {
    path: '',
    component: DashboardLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: DashboardShellComponent },
      ...identityRoutes,
      ...settingsRoutes,
      ...behavioursRoutes,
      ...frequenciesRoutes,
      ...commitmentsRoutes,
      ...trackingRoutes,
      ...notesRoutes,
      ...cardsRoutes,
    ]
  }
];
