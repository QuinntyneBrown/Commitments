// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ToDoService } from './to-do.service';
import { ToDosPageComponent } from 'to-dos-page/to-dos-page.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { EditToDoDialog } from 'edit-to-do-dialog/edit-to-do-dialog';
import { EditToDoDialogService } from './edit-to-do-dialog';
import { DashboardCardsModule } from '../dashboard-cards/dashboard-cards.module';
import { ToDoDashboardCardComponent } from 'to-do-dashboard-card/to-do-dashboard-card.component';

const declarations = [
  EditToDoDialog,
  ToDosPageComponent,
  ToDoDashboardCardComponent
];

const providers = [
  ToDoService,
  EditToDoDialogService
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

