// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ToDoService } from './to-do.service';
import { ToDosPage } from './to-dos-page';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { EditToDoOverlay } from './edit-to-do-overlay';
import { EditToDoOverlayService } from './edit-to-do-overlay';
import { DashboardCardsModule } from '../dashboard-cards/dashboard-cards.module';
import { ToDoDashboardCard } from './to-do-dashboard-card';

const declarations = [
  EditToDoOverlay,
  ToDosPage,
  ToDoDashboardCard
];

const providers = [
  ToDoService,
  EditToDoOverlayService
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    DashboardCardsModule,
    SharedModule
  ],
  providers,
  })
export class ToDosModule { }

