import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { CommitmentService } from './commitment.service';
import { MyCommimentsPageComponent } from './my-commiments-page.component';
import { EditCommitmentPageComponent } from './edit-commitment-page.component';
import { AddCommitmentsOverlayComponent } from './add-commitments-overlay.component';
import { BehavioursModule } from '../behaviours/behaviours.module';
import { CommitmentFrequencyEditorComponent } from './commitment-frequency-editor.component';
import { CommitmentFrequencyService } from './commitment-frequency.service';
import { FrequencyTypeService } from './frequency-type.service';

const declarations = [
  MyCommimentsPageComponent,
  EditCommitmentPageComponent,
  AddCommitmentsOverlayComponent,
  CommitmentFrequencyEditorComponent
];

const providers = [
  CommitmentService,
  CommitmentFrequencyService,
  FrequencyTypeService
];

const entryComponents = [
  AddCommitmentsOverlayComponent
];

@NgModule({
  declarations,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,

    BehavioursModule,
    CoreModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class CommitmentsModule { }
