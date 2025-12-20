// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./edit-behaviour-type-dialog.html",
  styleUrls: ["./edit-behaviour-type-dialog.scss"],
  selector: "app-edit-behaviour-type-dialog"
})
export class EditBehaviourTypeDialog {

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

