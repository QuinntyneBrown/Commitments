import { Component, Injector } from "@angular/core";
import { Subject } from "rxjs";
import { CommitmentService } from "./commitment.service";
import { Overlay } from "@angular/cdk/overlay";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { AddCommitmentsOverlayComponent } from "./add-commitments-overlay.component";
import { map, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./my-commiments-page.component.html",
  styleUrls: ["./my-commiments-page.component.css"],
  selector: "app-my-commiments-page"
})
export class MyCommimentsPageComponent { 
  constructor(
    private _commitmentService: CommitmentService,
    private _injector: Injector,
    private _overlay: Overlay
    
  ) { }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

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
    const overlayPortal = new ComponentPortal(AddCommitmentsOverlayComponent, null, injector);
    
    overlayRef.attach(overlayPortal);

    overlayRefWrapper.results
      .pipe(map(this.handleSelectedBehaviours),takeUntil(this.onDestroy))
      .subscribe()    
  }

  public handleSelectedBehaviours($data) {
    console.log($data);
  }
}
