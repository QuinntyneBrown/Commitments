import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardService } from './card.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CardsPageComponent } from './cards-page.component';
import { EditCardOverlayComponent } from './edit-card-overlay.component';
import { EditCardOverlay } from './edit-card-overlay';

const declarations = [
  CardsPageComponent,
  EditCardOverlayComponent
];

const providers = [
  CardService,
  EditCardOverlay
];

const entryComponents = [
  EditCardOverlayComponent
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
  entryComponents
})
export class CardsModule { }
