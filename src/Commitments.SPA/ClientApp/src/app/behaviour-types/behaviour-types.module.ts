import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehaviourTypesPageComponent } from './behaviour-types-page.component';
import { EditBehaviourTypeOverlayComponent } from './edit-behaviour-type-overlay.component';
import { EditBehaviourTypeOverlay } from './edit-behaviour-type-overlay';

const declarations = [
  BehaviourTypesPageComponent,
  EditBehaviourTypeOverlayComponent
];

const providers = [
  EditBehaviourTypeOverlay
];

const entryComponents = [
  EditBehaviourTypeOverlayComponent
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
export class BehaviourTypesModule { }
