// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule, WeekDay } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AchievementService } from './achievement.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CommitmentsModule } from '../commitments/commitments.module';
import { ActivitiesModule } from '../activities/activities.module';
import { DashboardCardsModule } from '../dashboard-cards/dashboard-cards.module';
import { DailyResultsDashboardCard } from './daily-results-dashboard-card';
import { WeeklyResultsDashboardCard } from './weekly-results-dashboard-card';
import { MonthlyResultsDashboardCard } from './monthly-results-dashboard-card';
import { RelationsResultsDashboardCard } from './relations-results-dashboard-card';

const declarations = [
  DailyResultsDashboardCard,
  WeeklyResultsDashboardCard,
  MonthlyResultsDashboardCard,
  RelationsResultsDashboardCard
];

const providers = [
  AchievementService
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    ActivitiesModule,
    CoreModule,
    CommitmentsModule,
    DashboardCardsModule,
    SharedModule,
  ],
  providers,
  exports: declarations
})
export class AchievementsModule { }

