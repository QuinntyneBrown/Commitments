// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { OverlayRefWrapper } from "../../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./edit-behaviour-type-overlay.html",
  styleUrls: ["./edit-behaviour-type-overlay.scss"],
  selector: "app-edit-behaviour-type-overlay"
})
export class EditBehaviourTypeOverlay {

  constructor(
    private readonly _overlay: OverlayRefWrapper
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
