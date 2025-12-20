// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, ComponentRef, Injector } from '@angular/core';
import { OverlayRefWrapper } from '../core/overlay-ref-wrapper';
import { ComponentPortal } from '@angular/cdk/portal';
import { EditToDoDialog } from 'edit-to-do-dialog/edit-to-do-dialog';
import { OverlayRefProvider } from '../core/overlay-ref-provider';
import { Observable } from 'rxjs';

@Injectable()
export class EditToDoDialogService {
  constructor(
    public _injector: Injector,
    public _overlayRefProvider: OverlayRefProvider
  ) {}

  public create(options: { toDoId?: number } = {}): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    overlayComponent.toDoId = options.toDoId;
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef: any, overlayRefWrapper: OverlayRefWrapper) {
    const injector = Injector.create({
      parent: this._injector,
      providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }],
    });
    const overlayPortal = new ComponentPortal(EditToDoDialog, null, injector);
    const overlayPortalRef: ComponentRef<EditToDoDialog> =
      overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}

