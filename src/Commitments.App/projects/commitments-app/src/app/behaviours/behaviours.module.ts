// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BehaviourService } from './behaviour.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehavioursPage } from './behaviours-page';
import { EditBehaviourOverlay as EditBehaviourOverlayService } from './edit-behaviour-overlay';
import { EditBehaviourOverlay as EditBehaviourOverlayComponent } from './edit-behaviour-overlay/edit-behaviour-overlay';
import { BehaviourTypesModule } from '../behaviour-types/behaviour-types.module';

const declarations = [
  BehavioursPage,
  EditBehaviourOverlayComponent
];

const providers = [
  BehaviourService,
  EditBehaviourOverlayService
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    BehaviourTypesModule,
    CoreModule,
    SharedModule
  ],
  providers,
  })
export class BehavioursModule { }

