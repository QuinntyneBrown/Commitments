import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BehaviourService } from './behaviour.service';
import { EditBehaviourPageComponent } from './edit-behaviour-page.component';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { BehaviourTypeService } from './behaviour-type.service';

const declarations = [
  EditBehaviourPageComponent
];

const providers = [
  BehaviourService,
  BehaviourTypeService
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
export class BehavioursModule { }
