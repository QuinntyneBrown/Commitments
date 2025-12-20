// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ActivityService } from './activity.service';
import { ActivitiesPage } from './activities-page/activities-page';
import { EditActivityOverlay as EditActivityOverlayComponent } from './edit-activity-overlay/edit-activity-overlay';
import { EditActivityOverlay as EditActivityOverlayService } from './edit-activity-overlay';

const declarations = [
  ActivitiesPage,
  EditActivityOverlayComponent
];

const providers = [
  ActivityService,
  EditActivityOverlayService
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

