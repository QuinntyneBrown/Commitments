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
import { FrequencyPageComponent } from './frequency-page.component';

const declarations = [
  FrequenciesEditorComponent,
  FrequencyEditorComponent,
  FrequencyPageComponent
];

const providers = [
  FrequencyService,
  FrequencyTypeService
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
})
export class FrequenciesModule { }
