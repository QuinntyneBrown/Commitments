import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DashboardPageComponent } from './dashboard-page.component';
import { ActivitiesModule } from '../activities/activities.module';
import { CommitmentsModule } from '../commitments/commitments.module';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';

const declarations = [
  DashboardPageComponent
];

const providers = [

];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    ActivitiesModule,
    CommitmentsModule,
    CoreModule,
    SharedModule
  ],
  providers,
})
export class DashboardModule { }
