// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ActivityService } from './activity.service';
import { ActivitiesPageComponent } from 'activities-page/activities-page.component';
import { EditActivityDialog } from 'edit-activity-dialog/edit-activity-dialog';
import { EditActivityDialogService } from './edit-activity-dialog';

const declarations = [
  ActivitiesPageComponent,
  EditActivityDialog
];

const providers = [
  ActivityService,
  EditActivityDialogService
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    SharedModule
  ],
  providers,
  exports: declarations,
  })
export class ActivitiesModule { }

