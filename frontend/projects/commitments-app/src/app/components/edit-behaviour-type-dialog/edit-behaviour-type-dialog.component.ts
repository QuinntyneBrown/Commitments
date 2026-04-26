// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';

@Component({
  selector: 'app-edit-behaviour-type-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-behaviour-type-dialog.html',
  styleUrls: ['./edit-behaviour-type-dialog.scss']
})
export class EditBehaviourTypeDialogComponent {
  private readonly _overlay = inject(OverlayRefWrapper);

  public onDestroy: Subject<void> = new Subject<void>();

  public behaviourTypeId: number;

  public handleSaveClick() {}

  public handleCancelClick() {
    this._overlay.close();
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
