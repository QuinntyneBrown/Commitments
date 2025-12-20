// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FrequenciesEditorComponent } from 'frequencies-editor/frequencies-editor.component';
import { FrequencyEditorComponent } from 'frequency-editor/frequency-editor.component';
import { FrequencyService } from './frequency.service';
import { FrequencyTypeService } from './frequency-type.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { EditFrequencyPageComponent } from 'edit-frequency-page/edit-frequency-page.component';
import { EditFrequencyDialog } from 'edit-frequency-dialog/edit-frequency-dialog';
import { FrequenciesPageComponent } from 'frequencies-page/frequencies-page.component';
import { EditFrequencyDialogService } from './edit-frequency-dialog';

const declarations = [
  FrequenciesEditorComponent,
  FrequencyEditorComponent,
  EditFrequencyPageComponent,
  EditFrequencyDialog,
  FrequenciesPageComponent
];

const providers = [
  FrequencyService,
  FrequencyTypeService,
  EditFrequencyDialogService
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
  exports: declarations,
  })
export class FrequenciesModule { }

