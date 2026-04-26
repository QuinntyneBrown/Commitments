// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export type StatusPillVariant = 'live' | 'review' | 'neutral';

@Component({
  selector: 'cui-status-pill',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './status-pill.component.html',
  styleUrls: ['./status-pill.component.scss']
})
export class StatusPillComponent {
  @Input() variant: StatusPillVariant = 'neutral';
  @Input() pulse = false;
  @Input() label = '';
}
