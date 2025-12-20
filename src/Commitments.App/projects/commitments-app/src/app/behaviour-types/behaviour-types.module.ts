// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehaviourTypesPage } from './behaviour-types-page/behaviour-types-page';
import { EditBehaviourTypeOverlay as EditBehaviourTypeOverlayComponent } from './edit-behaviour-type-overlay/edit-behaviour-type-overlay';
import { EditBehaviourTypeOverlay } from './edit-behaviour-type-overlay';
import { BehaviourTypeService } from './behaviour-type.service';

const declarations = [
  BehaviourTypesPage,
  EditBehaviourTypeOverlayComponent
];

const providers = [
  EditBehaviourTypeOverlay,
  BehaviourTypeService
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    SharedModule
  ],
  providers,
  })
export class BehaviourTypesModule { }

