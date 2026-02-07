// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, ComponentRef, Injector, inject } from '@angular/core';
import { OverlayRefWrapper } from '../core/overlay-ref-wrapper';
import { ComponentPortal } from '@angular/cdk/portal';
import { EditToDoDialogComponent } from '../components/edit-to-do-dialog/edit-to-do-dialog.component';
import { OverlayRefProvider } from '../core/overlay-ref-provider';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EditToDoDialogService {
  private readonly _injector = inject(Injector);
  private readonly _overlayRefProvider = inject(OverlayRefProvider);

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
    const overlayPortal = new ComponentPortal(EditToDoDialogComponent, null, injector);
    const overlayPortalRef: ComponentRef<EditToDoDialogComponent> =
      overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}
