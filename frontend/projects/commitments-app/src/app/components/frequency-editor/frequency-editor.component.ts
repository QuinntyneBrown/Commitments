// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Frequency } from '../../models/frequency';
import { FrequencyType } from '../../models/frequency-type';

@Component({
  selector: 'app-frequency-editor',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './frequency-editor.component.html',
  styleUrls: ['./frequency-editor.component.scss']
})
export class FrequencyEditorComponent {
  public frequency: number;

  public readonly frequencyTypes = input<Array<FrequencyType>>([]);

  public readonly save = output<{ frequency: Frequency }>();

  public handleSaveClick() {
    const frequency = new Frequency();
    frequency.frequency = this.form.value.frequency;
    frequency.frequencyTypeId = this.form.value.frequencyTypeId;
    frequency.isDesired = this.form.value.isDesired;
    this.save.emit({ frequency });
  }

  public form: FormGroup = new FormGroup({
    frequency: new FormControl(null, [Validators.required]),
    frequencyTypeId: new FormControl(null, [Validators.required]),
    isDesired: new FormControl(true, [Validators.required])
  });
}
