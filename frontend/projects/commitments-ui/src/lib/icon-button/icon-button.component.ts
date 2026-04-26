// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'cui-icon-button',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './icon-button.component.html',
  styleUrls: ['./icon-button.component.scss']
})
export class IconButtonComponent {
  @Input() icon = '';
  @Input() ariaLabel = '';
  @Input() disabled = false;
  @Input() pressed = false;
  @Output() pressedChange = new EventEmitter<boolean>();

  toggle(): void {
    if (this.disabled) return;
    this.pressed = !this.pressed;
    this.pressedChange.emit(this.pressed);
  }
}
