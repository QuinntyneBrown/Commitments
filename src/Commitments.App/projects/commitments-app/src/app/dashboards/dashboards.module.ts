// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DashboardPageComponent } from 'dashboard-page/dashboard-page.component';
import { ActivitiesModule } from '../activities/activities.module';
import { CommitmentsModule } from '../commitments/commitments.module';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AchievementsModule } from '../achievements/achievements.module';
import { DashboardService } from './dashboard.service';

const declarations = [
  DashboardPageComponent
];

const providers = [
  DashboardService
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    AchievementsModule,
    ActivitiesModule,
    CommitmentsModule,
    CoreModule,
    SharedModule
  ],
  providers,
})
export class DashboardsModule { }

