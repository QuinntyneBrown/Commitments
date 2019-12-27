import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./edit-behaviour-type-overlay.component.html",
  styleUrls: ["./edit-behaviour-type-overlay.component.css"],
  selector: "app-edit-behaviour-type-overlay"
})
export class EditBehaviourTypeOverlayComponent {

  constructor(
    private _overlay: OverlayRefWrapper
  ) {

  }

  public onDestroy: Subject<void> = new Subject<void>();

  public behaviourTypeId: number;

  public handleSaveClick() {
    
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
