// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { CommitmentService } from '../../services/commitment.service';
import { Commitment } from '../../models/commitment';
import { BehaviourService } from '../../services/behaviour.service';
import { Behaviour } from '../../models/behaviour';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { Frequency } from '../../models/frequency';
import { CommitmentFrequency } from '../../models/commitment-frequency';
import { FrequencyService } from '../../services/frequency.service';
import { map, takeUntil } from 'rxjs';

@Component({
  selector: 'app-edit-commitment-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-commitment-dialog.html',
  styleUrls: ['./edit-commitment-dialog.scss']
})
export class EditCommitmentDialogComponent {
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _behaviourService = inject(BehaviourService);
  private readonly _commitmentService = inject(CommitmentService);
  private readonly _frequencyService = inject(FrequencyService);

  ngOnInit() {
    this.behaviours$ = this._behaviourService.get();
    this.frequencies$ = this._frequencyService.get();
  }

  public commitmentId: number;

  private readonly _commitment = new Commitment();

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public onNgModelChange($event) {
    console.log($event);
  }

  public handleSaveClick(behaviours) {
    this._commitment.commitmentFrequencies = this.frequencies.selectedOptions.selected.map(x => new CommitmentFrequency(x.value.frequencyId, 0));
    this._commitment.behaviourId = this.behaviours.selectedOptions.selected.map(x => x.value.behaviourId)[0];

    this._commitmentService.save({ commitment: this._commitment })
      .pipe(map(x => {
        this._commitment.commitmentId = x.commitmentId;
        this._overlay.close(this._commitment);
      }), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleFrequenciesEditorChange(frequencies: Array<Frequency>) {
    //this._commitment.frequencies = frequencies.map(x => new CommitmentFrequency());
  }

  @ViewChild('behaviours')
  public behaviours: any;

  @ViewChild('frequencies')
  public frequencies: any;

  public behaviours$: Observable<Array<Behaviour>>;
  public frequencies$: Observable<Array<Frequency>>;
  public commitments$: Observable<Array<Commitment>>;

  public form: FormGroup = new FormGroup({
    behaviourId: new FormControl(null, []),
  });
}
