import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ToDoService } from './to-do.service';
import { ToDosPageComponent } from './to-dos-page.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { EditToDoOverlayComponent } from './edit-to-do-overlay.component';
import { EditToDoOverlay } from './edit-to-do-overlay';
import { DashboardCardsModule } from '../dashboard-cards/dashboard-cards.module';
import { ToDoDashboardCardComponent } from './to-do-dashboard-card.component';

const declarations = [
  EditToDoOverlayComponent,
  ToDosPageComponent,
  ToDoDashboardCardComponent
];

const entryComponents = [
  EditToDoOverlayComponent,
  ToDoDashboardCardComponent
];

const providers = [
  ToDoService,
  EditToDoOverlay
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    DashboardCardsModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class ToDosModule { }
