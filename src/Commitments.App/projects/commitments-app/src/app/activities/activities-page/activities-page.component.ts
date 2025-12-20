// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject, Observable, BehaviorSubject } from "rxjs";
import { takeUntil, tap, map } from "rxjs";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { ActivityService } from "./activity.service";
import { Activity } from "./activity";
import { deepCopy } from "../core/deep-copy";
import { GridApi, ColDef } from "ag-grid";
import { CheckboxCellComponent } from "../shared/checkbox-cell.component";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditCellComponent } from "../shared/edit-cell.component";
import { EditActivityDialogService } from "./edit-activity-dialog";

@Component({
  templateUrl: "./activities-page.component.html",
  styleUrls: ["./activities-page.component.scss"],
  selector: "app-activities-page"
})
export class ActivitiesPageComponent {
  constructor(
    private readonly _activityService: ActivityService,
    private readonly _editActityDialog: EditActivityDialogService,
    private readonly _router: Router
  ) { }

  ngOnInit() {
    this._activityService.get()
      .pipe(map(x => this.activities$.next(x)))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public activities$: BehaviorSubject<Array<Activity>> = new BehaviorSubject([]);

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleRemoveClick($event) {
    const activities: Array<Activity> = [...this.activities$.value];
    const index = activities.findIndex(x => x.activityId == $event.data.activityId);
    activities.splice(index, 1);
    this.activities$.next(activities);

    this._activityService.remove({ activity: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditClick($event) {
    this._editActityDialog.create({ activityId: $event.data.activityId })
      .pipe(takeUntil(this.onDestroy), map((x) => this.addOrUpdate(x)))
      .subscribe();
  }

  public handleFABButtonClick() {
    this._editActityDialog.create()
      .pipe(takeUntil(this.onDestroy), map((x) => this.addOrUpdate(x)))
      .subscribe();
  }

  public addOrUpdate(activity: Activity) {
    if (!activity) return;

    let activities = [...this.activities$.value];
    const i = activities.findIndex((t) => t.activityId == activity.activityId);
    const _ = i < 0 ? activities.push(activity) : activities[i] = activity;
    this.activities$.next(activities);
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Behaviour", field: "behaviour.name" },
    { headerName: "Performed On", field: "performedOn" },
    { cellRenderer: "editRenderer", onCellClicked: $event => this.handleEditClick($event), width: 30 },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemoveClick($event), width: 30 }
  ];

  public frameworkComponents: any = {
    checkboxRenderer: CheckboxCellComponent,
    deleteRenderer: DeleteCellComponent,
    editRenderer: EditCellComponent
  };

  private readonly _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
    this._gridApi.sizeColumnsToFit();
  }
}

