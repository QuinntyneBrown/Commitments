import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardsModule } from '../cards/cards.module';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { AddDashboardCardsOverlayComponent } from './add-dashboard-cards-overlay.component';
import { DashboardCardService } from './dashboard-card.service';

const declarations = [
  AddDashboardCardsOverlayComponent
];

const providers = [
  DashboardCardService
];

const entryComponents = [
  AddDashboardCardsOverlayComponent
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CardsModule,
    CoreModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class DashboardCardsModule { }
