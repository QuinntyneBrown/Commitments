import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardsModule } from '../cards/cards.module';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { DashboardCardService } from './dashboard-card.service';
import { DashboardCardConfigurationOverlay } from './dashboard-card-configuration-overlay';
import { AddDashboardCardsOverlay } from './add-dashboard-cards-overlay';
import { AddDashboardCardsOverlayComponent } from './add-dashboard-cards-overlay.component';

const declarations = [
  AddDashboardCardsOverlayComponent
];

const providers = [
  DashboardCardService,
  DashboardCardConfigurationOverlay,
  AddDashboardCardsOverlay
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
