import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardsPageComponent } from '../cards/cards-page.component';
import { EditCardOverlayComponent } from '../cards/edit-card-overlay.component';
import { CardService } from './card.service';
import { EditCardOverlay } from '../cards/edit-card-overlay';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';

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
