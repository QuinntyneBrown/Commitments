import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FrequenciesEditorComponent } from './frequencies-editor.component';
import { FrequencyEditorComponent } from './frequency-editor.component';
import { FrequencyService } from './frequency.service';
import { FrequencyTypeService } from './frequency-type.service';
import { CoreModule } from '../core/core.module';
import { SharedModule } from '../shared/shared.module';
import { EditFrequencyPageComponent } from './edit-frequency-page.component';
import { EditFrequencyOverlayComponent } from './edit-frequency-overlay.component';
import { FrequenciesPageComponent } from './frequencies-page.component';
import { EditFrequencyOverlay } from './edit-frequency-overlay';

const declarations = [
  FrequenciesEditorComponent,
  FrequencyEditorComponent,
  EditFrequencyPageComponent,
  EditFrequencyOverlayComponent,
  FrequenciesPageComponent
];

const providers = [
  FrequencyService,
  FrequencyTypeService,
  EditFrequencyOverlay
];

const entryComponents = [
  EditFrequencyOverlayComponent
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
  exports: declarations,
  entryComponents
})
export class FrequenciesModule { }
