// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Overlay, ScrollStrategyOptions, OverlayContainer, OverlayPositionBuilder, OverlayRef } from "@angular/cdk/overlay";
import { ComponentFactoryResolver, Injector, NgZone, Injectable } from "@angular/core";

@Injectable()
export class OverlayRefProvider {
  constructor(private readonly _overlay: Overlay) { }

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

