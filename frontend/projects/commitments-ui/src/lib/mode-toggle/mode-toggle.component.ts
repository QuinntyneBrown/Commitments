// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

import { DashboardMode } from '../tokens/tokens';

@Component({
  selector: 'cui-mode-toggle',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './mode-toggle.component.html',
  styleUrls: ['./mode-toggle.component.scss']
})
export class ModeToggleComponent {
  @Input() mode: DashboardMode = 'live';
  @Input() disabled = false;
  @Output() modeChange = new EventEmitter<DashboardMode>();

  select(next: DashboardMode): void {
    if (this.disabled || next === this.mode) return;
    this.mode = next;
    this.modeChange.emit(next);
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'ArrowLeft') this.select('live');
    else if (event.key === 'ArrowRight') this.select('review');
  }
}
