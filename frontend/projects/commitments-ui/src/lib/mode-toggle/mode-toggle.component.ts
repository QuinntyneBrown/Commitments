// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, input, model } from '@angular/core';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';

import { DashboardMode } from '../tokens/tokens';

@Component({
  selector: 'cui-mode-toggle',
  standalone: true,
  imports: [MatButtonToggleModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './mode-toggle.component.html',
  styleUrls: ['./mode-toggle.component.scss']
})
export class ModeToggleComponent {
  readonly mode = model<DashboardMode>('live');
  readonly disabled = input(false);

  select(next: DashboardMode): void {
    if (this.disabled() || next === this.mode()) return;
    this.mode.set(next);
  }

  onKeyDown(event: KeyboardEvent): void {
    if (event.key === 'ArrowLeft') this.select('live');
    else if (event.key === 'ArrowRight') this.select('review');
  }
}
