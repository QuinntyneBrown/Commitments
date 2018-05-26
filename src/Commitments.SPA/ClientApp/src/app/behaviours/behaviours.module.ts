import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BehaviourService } from './behaviour.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehavioursPageComponent } from './behaviours-page.component';
import { EditBehaviourOverlay } from './edit-behaviour-overlay';
import { EditBehaviourOverlayComponent } from './edit-behaviour-overlay.component';
import { BehaviourTypesModule } from '../behaviour-types/behaviour-types.module';

const declarations = [
  BehavioursPageComponent,
  EditBehaviourOverlayComponent
];

const providers = [
  BehaviourService,
  EditBehaviourOverlay
];

const entryComponents = [
  EditBehaviourOverlayComponent
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    BehaviourTypesModule,
    CoreModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class BehavioursModule { }
