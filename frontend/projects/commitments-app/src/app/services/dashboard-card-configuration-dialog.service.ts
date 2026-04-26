// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, ComponentRef, Injector, inject } from "@angular/core";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ComponentPortal } from "@angular/cdk/portal";
import { DashboardCardConfigurationDialogComponent } from "../components/dashboard-card-configuration-dialog/dashboard-card-configuration-dialog.component";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { DashboardCard } from "../models/dashboard-card";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class DashboardCardConfigurationDialogService {
  private readonly _injector = inject(Injector);
  private readonly _overlayRefProvider = inject(OverlayRefProvider);

  public create(options: { dashboardCard: DashboardCard }): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    overlayComponent.dashboardCard = options.dashboardCard;
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    const injector = Injector.create({ parent: this._injector, providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }] });
    const overlayPortal = new ComponentPortal(DashboardCardConfigurationDialogComponent, null, injector);
    const overlayPortalRef: ComponentRef<DashboardCardConfigurationDialogComponent> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}
