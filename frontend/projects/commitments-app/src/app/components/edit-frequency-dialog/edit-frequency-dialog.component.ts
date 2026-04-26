// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, Observable } from 'rxjs';
import { FrequencyType } from '../../models/frequency-type';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { FrequencyTypeService } from '../../services/frequency-type.service';
import { FrequencyService } from '../../services/frequency.service';
import { map } from 'rxjs';

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

  ngOnInit() {
    this.frequencyTypes$ = this.frequencyTypeService.get();
  }

  public frequencyId: number;

  public frequencyTypes$: Observable<Array<FrequencyType>>;

  public handleSave($event) {
    this.frequencyService.save({ frequency: $event.frequency })
      .pipe(map(x => {
        $event.frequency.frequencyId = x.frequencyId;
        this._overlay.close($event.frequency);
      }))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
