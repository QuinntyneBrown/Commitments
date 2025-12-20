// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardsModule } from '../cards/cards.module';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { DashboardCardService } from './dashboard-card.service';
import { DashboardCardConfigurationDialogService } from './dashboard-card-configuration-dialog';
import { AddDashboardCardsDialogService } from './add-dashboard-cards-dialog';
import { AddDashboardCardsDialog } from './add-dashboard-cards-dialog/add-dashboard-cards-dialog';
import { DashboardCardConfigurationDialog } from './dashboard-card-configuration-dialog/dashboard-card-configuration-dialog';
import { DashboardCardComponent } from 'dashboard-card/dashboard-card.component';
import { PosterDashboardCardComponent } from 'poster-dashboard-card/poster-dashboard-card.component';
import { DashboardCard } from './dashboard-card';

const declarations = [
  AddDashboardCardsDialog,
  DashboardCardConfigurationDialog,
  DashboardCardComponent,
  PosterDashboardCardComponent
];

const providers = [
  DashboardCardService,
  DashboardCardConfigurationDialogService,
  AddDashboardCardsDialogService
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CardsModule,
    CoreModule,
    SharedModule
  ],
  providers,
  })
export class DashboardCardsModule { }

