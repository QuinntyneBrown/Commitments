import { Injectable, ComponentRef, Injector } from "@angular/core";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { PortalInjector, ComponentPortal } from "@angular/cdk/portal";
import { EditProfileOverlayComponent } from "./edit-profile-overlay.component";
import { OverlayRefProvider } from "../core/overlay-ref-provider";
import { Observable } from "rxjs";

@Injectable()
export class EditProfileOverlay {
  constructor(
    public _injector: Injector,
    public _overlayRefProvider: OverlayRefProvider
  ) { }

  public create(options: { profileId?: number } = {}): Observable<any> {
    const overlayRef = this._overlayRefProvider.create();
    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    const overlayComponent = this.attachOverlayContainer(overlayRef, overlayRefWrapper);
    return overlayRefWrapper.afterClosed();
  }

  public attachOverlayContainer(overlayRef, overlayRefWrapper) {
    const injectionTokens = new WeakMap();
    injectionTokens.set(OverlayRefWrapper, overlayRefWrapper);
    const injector = new PortalInjector(this._injector, injectionTokens);
    const overlayPortal = new ComponentPortal(EditProfileOverlayComponent, null, injector);
    const overlayPortalRef: ComponentRef<EditProfileOverlayComponent> = overlayRef.attach(overlayPortal);
    return overlayPortalRef.instance;
  }
}
