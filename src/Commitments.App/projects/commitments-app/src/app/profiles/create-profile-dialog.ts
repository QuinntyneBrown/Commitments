// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, ComponentRef, Injector } from "@angular/core";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ComponentPortal } from "@angular/cdk/portal";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { Observable } from "rxjs";
import { CreateProfileDialog } from "create-profile-dialog/create-profile-dialog";

@Injectable()
export class CreateProfileDialogService {
  constructor(
    public _injector: Injector,
    public _overlayRefProvider: OverlayRefProvider
  ) { }

  public create(): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    // Updated to use Injector.create() instead of deprecated PortalInjector
    const injector = Injector.create({ parent: this._injector, providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }] });
    const overlayPortal = new ComponentPortal(CreateProfileDialog, null, injector);
    const overlayPortalRef: ComponentRef<CreateProfileDialog> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}

