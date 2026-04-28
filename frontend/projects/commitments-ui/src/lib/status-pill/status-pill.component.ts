// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatChipsModule } from '@angular/material/chips';

export type StatusPillVariant = 'live' | 'review' | 'neutral' | 'chart' | 'success' | 'warning';

@Component({
  selector: 'cui-status-pill',
  standalone: true,
  imports: [MatChipsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './status-pill.component.html',
  styleUrls: ['./status-pill.component.scss']
})
export class StatusPillComponent {
  readonly variant = input<StatusPillVariant>('neutral');
  readonly pulse = input(false);
  readonly label = input('');
}
