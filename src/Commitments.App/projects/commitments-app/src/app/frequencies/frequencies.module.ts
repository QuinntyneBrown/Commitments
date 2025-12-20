// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FrequenciesEditor } from './frequencies-editor';
import { FrequencyEditor } from './frequency-editor';
import { FrequencyService } from './frequency.service';
import { FrequencyTypeService } from './frequency-type.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { EditFrequencyPage } from './edit-frequency-page';
import { EditFrequencyOverlay } from './edit-frequency-overlay/';
import { FrequenciesPage } from './frequencies-page';
import { EditFrequencyOverlay as EditFrequencyOverlayService } from './edit-frequency-overlay';

const declarations = [
  FrequenciesEditor,
  FrequencyEditor,
  EditFrequencyPage,
  EditFrequencyOverlay,
  FrequenciesPage
];

const providers = [
  FrequencyService,
  FrequencyTypeService,
  EditFrequencyOverlayService
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

