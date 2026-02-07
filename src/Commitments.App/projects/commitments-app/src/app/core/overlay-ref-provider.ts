// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { Injectable, inject } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class OverlayRefProvider {
  private readonly _overlay = inject(Overlay);

  public create(): OverlayRef {
    const positionStrategy = this._overlay.position()
      .global()
      .centerHorizontally()
      .centerVertically();

    return this._overlay.create({
      hasBackdrop: true,
      positionStrategy
    });
  }
}
