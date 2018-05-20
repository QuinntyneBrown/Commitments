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
import { FrequenciesModule } from '../frequencies/frequencies.module';

const declarations = [
  MyCommimentsPageComponent,
  EditCommitmentPageComponent,
  AddCommitmentsOverlayComponent
];

const providers = [
  CommitmentService
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
    FrequenciesModule,
    SharedModule
  ],
  providers,
  entryComponents
})
export class CommitmentsModule { }
