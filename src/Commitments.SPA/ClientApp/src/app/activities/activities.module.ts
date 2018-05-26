import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { ActivityService } from './activity.service';
import { ActivitiesPageComponent } from './activities-page.component';
import { EditActivityOverlayComponent } from './edit-activity-overlay.component';
import { EditActivityOverlay } from './edit-activity-overlay';

const declarations = [
  ActivitiesPageComponent,
  EditActivityOverlayComponent
];

const providers = [
  ActivityService,
  EditActivityOverlay
];

const entryComponents = [
  EditActivityOverlayComponent
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
  exports:declarations,
  entryComponents
})
export class ActivitiesModule { }
