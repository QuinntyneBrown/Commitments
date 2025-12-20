// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardService } from './card.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CardsPageComponent } from 'cards-page/cards-page.component';
import { EditCardDialog } from 'edit-card-dialog/edit-card-dialog';
import { EditCardDialogService } from './edit-card-dialog';

const declarations = [
  CardsPageComponent,
  EditCardDialog
];

const providers = [
  CardService,
  EditCardDialogService
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

