// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { DashboardMode } from '../tokens/tokens';

export type MetricHeaderAccent = DashboardMode | 'chart';

@Component({
  selector: 'cui-metric-header',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './metric-header.component.html',
  styleUrls: ['./metric-header.component.scss']
})
export class MetricHeaderComponent {
  @Input() value = '';
  @Input() caption = '';
  @Input() accent: MetricHeaderAccent = 'chart';
}
