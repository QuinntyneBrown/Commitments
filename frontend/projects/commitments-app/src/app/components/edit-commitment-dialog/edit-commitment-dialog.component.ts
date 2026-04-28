// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, viewChild } from '@angular/core';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { CommitmentService } from '../../services/commitment.service';
import { Commitment } from '../../models/commitment';
import { BehaviourService } from '../../services/behaviour.service';
import { Behaviour } from '../../models/behaviour';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { Frequency } from '../../models/frequency';
import { CommitmentFrequency } from '../../models/commitment-frequency';
import { FrequencyService } from '../../services/frequency.service';
import { tap } from 'rxjs';

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
  private readonly _destroyRef = inject(DestroyRef);

  public readonly behaviours = toSignal(this._behaviourService.get(), { initialValue: [] as Array<Behaviour> });
  public readonly frequencies = toSignal(this._frequencyService.get(), { initialValue: [] as Array<Frequency> });

  public commitmentId: number;

  private readonly _commitment = new Commitment();

  public handleCancelClick() {
    this._overlay.close();
  }

  public onNgModelChange($event) {
    console.log($event);
  }

  public handleSaveClick() {
    this._commitment.commitmentFrequencies = this.frequenciesList()?.selectedOptions.selected.map(x => new CommitmentFrequency(x.value.frequencyId, 0));
    this._commitment.behaviourId = this.behavioursList()?.selectedOptions.selected.map(x => x.value.behaviourId)[0];

    this._commitmentService.save({ commitment: this._commitment })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => {
          this._commitment.commitmentId = x.commitmentId;
          this._overlay.close(this._commitment);
        })
      )
      .subscribe();
  }

  public handleFrequenciesEditorChange(frequencies: Array<Frequency>) {
    //this._commitment.frequencies = frequencies.map(x => new CommitmentFrequency());
  }

  public readonly behavioursList = viewChild<any>('behaviours');
  public readonly frequenciesList = viewChild<any>('frequencies');

  public form: FormGroup = new FormGroup({
    behaviourId: new FormControl(null, []),
  });
}
