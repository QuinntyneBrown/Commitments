// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, ComponentRef, Injector, inject } from "@angular/core";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ComponentPortal } from "@angular/cdk/portal";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { Observable } from "rxjs";
import { CreateProfileDialogComponent } from "../components/create-profile-dialog/create-profile-dialog.component";

@Injectable({ providedIn: 'root' })
export class CreateProfileDialogService {
  private readonly _injector = inject(Injector);
  private readonly _overlayRefProvider = inject(OverlayRefProvider);

  public create(): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    const injector = Injector.create({ parent: this._injector, providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }] });
    const overlayPortal = new ComponentPortal(CreateProfileDialogComponent, null, injector);
    const overlayPortalRef: ComponentRef<CreateProfileDialogComponent> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}
