// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { FrequencyType } from "./frequency-type";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { FrequencyTypeService } from "./frequency-type.service";
import { FrequencyService } from "./frequency.service";
import { takeUntil, map } from "rxjs";

@Component({
  templateUrl: "./edit-frequency-dialog.html",
  styleUrls: ["./edit-frequency-dialog.scss"],
  selector: "app-edit-frequency-dialog"
})
export class EditFrequencyDialog {
  constructor(public _overlay: OverlayRefWrapper,
    public frequencyTypeService: FrequencyTypeService,
    public frequencyService: FrequencyService
  ) { }

  ngOnInit() {
    this.frequencyTypes$ = this.frequencyTypeService.get();
  }

  public frequencyId: number;

  public frequencyTypes$: Observable<Array<FrequencyType>>;

  public handleSave($event) {
    this.frequencyService.save({ frequency: $event.frequency })
      .pipe(map(x => {
        $event.frequency.frequencyId = x.frequencyId;
        this._overlay.close($event.frequency);
      }))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

