import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BehaviourService } from './behaviour.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehaviourTypeService } from './behaviour-type.service';
import { BehavioursPageComponent } from './behaviours-page.component';
import { EditBehaviourOverlay } from './edit-behaviour-overlay';
import { EditBehaviourOverlayComponent } from './edit-behaviour-overlay.component';

const declarations = [
  BehavioursPageComponent,
  EditBehaviourOverlayComponent
];

const providers = [
  BehaviourService,
  BehaviourTypeService,
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

    CoreModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class BehavioursModule { }
