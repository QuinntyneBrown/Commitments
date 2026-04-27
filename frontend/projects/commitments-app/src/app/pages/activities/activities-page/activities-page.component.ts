// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid';
import { ActivityService } from '../../../services/activity.service';
import { Activity } from '../../../models/activity';
import { EditActivityDialogService } from '../../../services/edit-activity-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-activities-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './activities-page.component.html',
  styleUrls: ['./activities-page.component.scss']
})
export class ActivitiesPageComponent {
  private readonly _activityService = inject(ActivityService);
  private readonly _editActityDialog = inject(EditActivityDialogService);
  private readonly _router = inject(Router);

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

  private _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
    this._gridApi.sizeColumnsToFit();
  }
}
