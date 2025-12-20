// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehaviourTypesPageComponent } from 'behaviour-types-page/behaviour-types-page.component';
import { EditBehaviourTypeDialog } from 'edit-behaviour-type-dialog/edit-behaviour-type-dialog';
import { EditBehaviourTypeDialogService } from './edit-behaviour-type-dialog';
import { BehaviourTypeService } from './behaviour-type.service';

const declarations = [
  BehaviourTypesPageComponent,
  EditBehaviourTypeDialog
];

const providers = [
  EditBehaviourTypeDialogService,
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

