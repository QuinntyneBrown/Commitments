// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, ComponentRef, Injector } from "@angular/core";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ComponentPortal } from "@angular/cdk/portal";
import { EditCardDialog } from "edit-card-dialog/edit-card-dialog";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { Observable } from "rxjs";


@Injectable()
export class EditCardDialogService {
  constructor(
    public _injector: Injector,
    public _overlayRefProvider: OverlayRefProvider
  ) { }

  public create(options: { cardId?: number } = {}): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    overlayComponent.cardId = options.cardId;
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    // Updated to use Injector.create() instead of deprecated PortalInjector
    const injector = Injector.create({ parent: this._injector, providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }] });
    const overlayPortal = new ComponentPortal(EditCardDialog, null, injector);
    const overlayPortalRef: ComponentRef<EditCardDialog> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}

