// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BehaviourService } from './behaviour.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehavioursPageComponent } from 'behaviours-page/behaviours-page.component';
import { EditBehaviourDialogService } from './edit-behaviour-dialog';
import { EditBehaviourDialog } from 'edit-behaviour-dialog/edit-behaviour-dialog';
import { BehaviourTypesModule } from '../behaviour-types/behaviour-types.module';

const declarations = [
  BehavioursPageComponent,
  EditBehaviourDialog
];

const providers = [
  BehaviourService,
  EditBehaviourDialogService
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

