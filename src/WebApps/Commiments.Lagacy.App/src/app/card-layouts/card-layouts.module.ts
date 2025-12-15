// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardsPageComponent } from '../cards/cards-page/cards-page.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CardLayoutsPageComponent } from 'card-layouts-page/card-layouts-page.component';
import { CardLayoutService } from './card-layout.service';
import { EditCardLayoutOverlay } from './edit-card-layout-overlay';
import { EditCardLayoutOverlayComponent } from 'edit-card-layout-overlay/edit-card-layout-overlay.component';

const declarations = [
  CardLayoutsPageComponent,
  EditCardLayoutOverlayComponent
];

const providers = [
  CardLayoutService,
  EditCardLayoutOverlay
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

