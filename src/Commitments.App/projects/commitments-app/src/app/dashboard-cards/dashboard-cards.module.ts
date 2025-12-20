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
import { DashboardCardConfigurationOverlayService } from './dashboard-card-configuration-overlay';
import { AddDashboardCardsOverlayService } from './add-dashboard-cards-overlay';
import { AddDashboardCardsOverlay } from './add-dashboard-cards-overlay';
import { DashboardCardConfigurationOverlay } from './dashboard-card-configuration-overlay';
import { DashboardCard } from './dashboard-card';
import { PosterDashboardCard } from './poster-dashboard-card';

const declarations = [
  AddDashboardCardsOverlay,
  DashboardCardConfigurationOverlay,
  DashboardCard,
  PosterDashboardCard
];

const providers = [
  DashboardCardService,
  DashboardCardConfigurationOverlayService,
  AddDashboardCardsOverlayService
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

