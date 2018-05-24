import { Component, Injector } from "@angular/core";
import { Subject } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { Overlay } from "@angular/cdk/overlay";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { EditActivityOverlayComponent } from "./edit-activity-overlay.component";
import { map, takeUntil, tap } from "rxjs/operators";
import { Router } from "@angular/router";

@Component({
  templateUrl: "./activities-page.component.html",
  styleUrls: ["./activities-page.component.css"],
  selector: "app-activities-page"
})
export class ActivitiesPageComponent { 
  constructor(
    private _overlay: Overlay,
    private _injector: Injector,
    private _router: Router
  ) {

  }

  public onDestroy: Subject<void> = new Subject<void>();

  public handleFabButtonClick() {
    const positionStrategy = this._overlay
      .position()
      .global()
      .centerHorizontally()
      .centerVertically();

    const overlayRef = this._overlay.create({
      hasBackdrop: true,
      positionStrategy
    });

    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);

    const injectionTokens = new WeakMap();
    injectionTokens.set(OverlayRefWrapper, overlayRefWrapper);
    const injector = new PortalInjector(this._injector, injectionTokens);
    const overlayPortal = new ComponentPortal(EditActivityOverlayComponent, null, injector);

    overlayRef.attach(overlayPortal);

    overlayRefWrapper.afterClosed
      .pipe(takeUntil(this.onDestroy), tap(() => this._router.navigateByUrl("/")))
      .subscribe();
  }

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}
