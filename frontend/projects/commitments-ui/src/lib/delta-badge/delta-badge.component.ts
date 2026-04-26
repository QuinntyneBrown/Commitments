// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

export type DeltaBadgeFormat = 'percent' | 'count';
export type DeltaBadgeTone = 'positive' | 'negative' | 'neutral';

@Component({
  selector: 'cui-delta-badge',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './delta-badge.component.html',
  styleUrls: ['./delta-badge.component.scss']
})
export class DeltaBadgeComponent {
  @Input() delta = 0;
  @Input() format: DeltaBadgeFormat = 'percent';
  @Input() caption = '';

  formatted(): string {
    const suffix = this.format === 'percent' ? '%' : '';
    if (this.delta > 0) return `+${this.delta}${suffix}`;
    if (this.delta < 0) return `${this.delta}${suffix}`;
    return `±0${suffix}`;
  }

  tone(): DeltaBadgeTone {
    if (this.delta > 0) return 'positive';
    if (this.delta < 0) return 'negative';
    return 'neutral';
  }
}
