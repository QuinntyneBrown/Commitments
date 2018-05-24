import { Component, Injector } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { Overlay } from "@angular/cdk/overlay";
import { FrequencyService } from "./frequency.service";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { EditFrequencyOverlayComponent } from "./edit-frequency-overlay.component";
import { takeUntil, map } from "rxjs/operators";
import { FrequencyTypeService } from "./frequency-type.service";
import { FrequencyType } from "./frequency-type.model";

@Component({
  templateUrl: "./edit-frequency-page.component.html",
  styleUrls: ["./edit-frequency-page.component.css"],
  selector: "app-edit-frequency-page"
})
export class EditFrequencyPageComponent { 
  constructor(
    private _frequencyService: FrequencyService,
    private _frequencyTypeService: FrequencyTypeService,
    private _overlay: Overlay,
    private _injector: Injector
  ) { }

  frequencyTypes: Array<FrequencyType>;

  ngOnInit() {
    this._frequencyTypeService.get()
      .pipe(map(x => this.frequencyTypes = x), takeUntil(this.onDestroy))
      .subscribe();  
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
    overlayRefWrapper.data = {
      frequencyTypes: this.frequencyTypes
    };

    const injectionTokens = new WeakMap();
    injectionTokens.set(OverlayRefWrapper, overlayRefWrapper);    
    const injector = new PortalInjector(this._injector, injectionTokens);
    const overlayPortal = new ComponentPortal(EditFrequencyOverlayComponent, null, injector);

    overlayRef.attach(overlayPortal);

    overlayRefWrapper.afterClosed
      .pipe(takeUntil(this.onDestroy), map(x => console.log(x)))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();    
  }
}
