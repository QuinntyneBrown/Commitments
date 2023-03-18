// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { createEditCommitmentViewModel } from './create-edit-commitment-view-model';
import { PushModule } from '@ngrx/component';

@Component({
  selector: 'app-edit-commitment',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, PushModule],
  templateUrl: './edit-commitment.component.html',
  styleUrls: ['./edit-commitment.component.scss']
})
export class EditCommitmentComponent {
  public vm$ = createEditCommitmentViewModel();
}
