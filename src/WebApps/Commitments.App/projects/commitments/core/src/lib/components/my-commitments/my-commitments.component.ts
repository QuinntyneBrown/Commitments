// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { createMyCommitmentsViewModel } from './create-my-commitments-view-model';
import { PushModule } from '@ngrx/component';

@Component({
  selector: 'app-my-commitments',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, PushModule],
  templateUrl: './my-commitments.component.html',
  styleUrls: ['./my-commitments.component.scss']
})
export class MyCommitmentsComponent {
  public vm$ = createMyCommitmentsViewModel();
}
