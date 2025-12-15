// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { DigitalAssetService } from './digital-asset.service';
import { DigitalAssetInputUrlComponent } from 'digital-asset-url-input/digital-asset-url-input.component';

const declarations = [
  DigitalAssetInputUrlComponent
];

const providers = [
  DigitalAssetService
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
  exports: declarations
})
export class DigitalAssetsModule { }

