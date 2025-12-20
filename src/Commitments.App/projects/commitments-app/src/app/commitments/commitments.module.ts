// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CommitmentService } from './commitment.service';
import { MyCommimentsPage } from './my-commiments-page/my-commiments-page';
import { BehavioursModule } from '../behaviours/behaviours.module';
import { FrequenciesModule } from '../frequencies/frequencies.module';
import { EditCommitmentOverlay as EditCommitmentOverlayComponent } from './edit-commitment-overlay/edit-commitment-overlay';
import { EditCommitmentOverlay as EditCommitmentOverlayService } from './edit-commitment-overlay';

const declarations = [
  MyCommimentsPage,
  EditCommitmentOverlayComponent
];

const providers = [
  CommitmentService,
  EditCommitmentOverlayService
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    BehavioursModule,
    CoreModule,
    FrequenciesModule,
    SharedModule
  ],
  providers,
  })
export class CommitmentsModule { }

