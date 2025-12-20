// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardService } from './card.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CardsPage } from './cards-page';
import { EditCardOverlay } from './edit-card-overlay';
import { EditCardOverlayService } from './edit-card-overlay.service';

const declarations = [
  CardsPage,
  EditCardOverlay
];

const providers = [
  CardService,
  EditCardOverlayService
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
export class CardsModule { }

