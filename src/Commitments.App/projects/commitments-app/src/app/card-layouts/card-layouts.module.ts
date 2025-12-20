// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardsPage } from '../cards/cards-page';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CardLayoutsPage } from './card-layouts-page';
import { CardLayoutService } from './card-layout.service';
import { EditCardLayoutOverlayService } from './edit-card-layout-overlay';
import { EditCardLayoutOverlay } from './edit-card-layout-overlay';

const declarations = [
  CardLayoutsPage,
  EditCardLayoutOverlay
];

const providers = [
  CardLayoutService,
  EditCardLayoutOverlayService
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
  })
export class CardLayoutsModule { }

