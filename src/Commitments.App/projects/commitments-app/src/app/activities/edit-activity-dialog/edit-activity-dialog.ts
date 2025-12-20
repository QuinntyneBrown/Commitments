// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { BehaviourService } from "../behaviours/behaviour.service";
import { Behaviour } from "../behaviours/behaviour";
import { ActivityService } from "./activity.service";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { Activity } from "./activity";
import { takeUntil, map } from "rxjs";
import { FormGroup, FormControl, Validators } from "@angular/forms";

@Component({
  templateUrl: "./edit-activity-dialog.html",
  styleUrls: ["./edit-activity-dialog.scss"],
  selector: "app-edit-activity-dialog"
})
export class EditActivityDialog {
  constructor(
    private readonly _activityService: ActivityService,
    private readonly _behaviourService: BehaviourService,
    private readonly _overlay: OverlayRefWrapper
  ) { }

  public activityId: number;

  public handleSaveClick() {
    let activity = new Activity();

    activity.activityId = this.activityId;
    activity.behaviourId = this.form.value.behaviourId;
    activity.performedOn = this.form.value.performedOn;
    activity.description = this.form.value.description;

    this._activityService.save({ activity })
      .pipe(map((x: { activityId: number }) => {
        activity.activityId = x.activityId;
        this._overlay.close(activity);
      }), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleCancelClick() {
    this._overlay.close();
  }
  ngOnInit() {
    this.behaviours$ = this._behaviourService.get();

    this._activityService.getById({ activityId: this.activityId })
      .pipe(map(x => this.form.patchValue({
        performedOn: x.performedOn,
        behaviourId: x.behaviourId,
        description: x.description
      })))
      .subscribe();
  }

  public behaviours$: Observable<Array<Behaviour>>;

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public form: FormGroup = new FormGroup({
    performedOn: new FormControl(null, [Validators.required]),
    behaviourId: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [])
  });
}

