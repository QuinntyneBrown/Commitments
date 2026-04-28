// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, Input, input, output, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'cui-icon-button',
  standalone: true,
  imports: [MatButtonModule, MatIconModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './icon-button.component.html',
  styleUrls: ['./icon-button.component.scss']
})
export class IconButtonComponent {
  readonly icon = input('');
  readonly ariaLabel = input('');
  readonly disabled = input(false);
  readonly pressed = signal(false);
  readonly pressedChange = output<boolean>();

  @Input({ alias: 'pressed' }) set pressedInput(value: boolean) {
    this.pressed.set(Boolean(value));
  }

  toggle(): void {
    if (this.disabled()) return;
    const next = !this.pressed();
    this.pressed.set(next);
    this.pressedChange.emit(next);
  }
}
