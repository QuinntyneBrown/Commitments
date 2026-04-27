// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, Input, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
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
  public onDestroy: Subject<void> = new Subject<void>();

  public frequency: number;

  @Input()
  public frequencyTypes: Array<FrequencyType> = [];

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleSaveClick() {
    const frequency = new Frequency();
    frequency.frequency = this.form.value.frequency;
    frequency.frequencyTypeId = this.form.value.frequencyTypeId;
    frequency.isDesired = this.form.value.isDesired;
    this.save.emit({ frequency });
  }

  @Output()
  public save: EventEmitter<any> = new EventEmitter();

  public form: FormGroup = new FormGroup({
    frequency: new FormControl(null, [Validators.required]),
    frequencyTypeId: new FormControl(null, [Validators.required]),
    isDesired: new FormControl(true, [Validators.required])
  });
}
