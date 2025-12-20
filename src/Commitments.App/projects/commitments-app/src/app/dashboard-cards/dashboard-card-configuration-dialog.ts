// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, ComponentRef, Injector } from "@angular/core";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { ComponentPortal } from "@angular/cdk/portal";
import { DashboardCardConfigurationDialog } from "./dashboard-card-configuration-dialog/dashboard-card-configuration-dialog";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { DashboardCard } from "./dashboard-card";
import { Dashboard } from "../dashboards/dashboard";
import { Observable } from "rxjs";

@Injectable()
export class DashboardCardConfigurationDialogService {
  constructor(
    public _injector: Injector,
    public _overlayRefProvider: OverlayRefProvider
  ) { }

  public create(options: { dashboardCard: DashboardCard }): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    overlayComponent.dashboardCard = options.dashboardCard;
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    // Updated to use Injector.create() instead of deprecated PortalInjector
    const injector = Injector.create({ parent: this._injector, providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }] });
    const overlayPortal = new ComponentPortal(DashboardCardConfigurationDialog, null, injector);
    const overlayPortalRef: ComponentRef<DashboardCardConfigurationDialog> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}

