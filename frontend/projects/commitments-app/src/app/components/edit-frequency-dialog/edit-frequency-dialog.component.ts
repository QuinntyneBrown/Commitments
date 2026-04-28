// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FrequencyType } from '../../models/frequency-type';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { FrequencyTypeService } from '../../services/frequency-type.service';
import { FrequencyService } from '../../services/frequency.service';
import { tap } from 'rxjs';

@Component({
  selector: 'app-edit-frequency-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-frequency-dialog.html',
  styleUrls: ['./edit-frequency-dialog.scss']
})
export class EditFrequencyDialogComponent {
  public _overlay = inject(OverlayRefWrapper);
  public frequencyTypeService = inject(FrequencyTypeService);
  public frequencyService = inject(FrequencyService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly frequencyTypes = toSignal(this.frequencyTypeService.get(), { initialValue: [] as Array<FrequencyType> });

  public frequencyId: number;

  public handleSave($event) {
    this.frequencyService.save({ frequency: $event.frequency })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => {
          $event.frequency.frequencyId = x.frequencyId;
          this._overlay.close($event.frequency);
        })
      )
      .subscribe();
  }
}
