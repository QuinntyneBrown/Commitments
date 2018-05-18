import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { BehaviourService } from './behaviour.service';
import { EditBehaviourPageComponent } from './edit-behaviour-page.component';

const declarations = [
  EditBehaviourPageComponent
];

const providers = [
  BehaviourService
];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule	
  ],
  providers,
})
export class BehavioursModule { }
