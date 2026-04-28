// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { MatChipsModule } from '@angular/material/chips';

export type DeltaBadgeFormat = 'percent' | 'count';
export type DeltaBadgeTone = 'positive' | 'negative' | 'neutral';

@Component({
  selector: 'cui-delta-badge',
  standalone: true,
  imports: [MatChipsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './delta-badge.component.html',
  styleUrls: ['./delta-badge.component.scss']
})
export class DeltaBadgeComponent {
  readonly delta = input(0);
  readonly format = input<DeltaBadgeFormat>('percent');
  readonly caption = input('');

  readonly formatted = computed(() => {
    const value = this.delta();
    const suffix = this.format() === 'percent' ? '%' : '';
    if (value > 0) return `+${value}${suffix}`;
    if (value < 0) return `${value}${suffix}`;
    return `±0${suffix}`;
  });

  readonly tone = computed<DeltaBadgeTone>(() => {
    const value = this.delta();
    if (value > 0) return 'positive';
    if (value < 0) return 'negative';
    return 'neutral';
  });

  readonly icon = computed(() => {
    const t = this.tone();
    if (t === 'positive') return 'arrow_upward';
    if (t === 'negative') return 'arrow_downward';
    return '';
  });
}
