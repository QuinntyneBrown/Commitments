// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid-community';
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
  private readonly _destroyRef = inject(DestroyRef);

  public readonly activities = signal<Array<Activity>>([]);

  public localeText: any = {};

  ngOnInit() {
    this._activityService.get()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.activities.set(x)))
      .subscribe();
  }

  public handleRemoveClick($event) {
    this.activities.update(activities => activities.filter(x => x.activityId != $event.data.activityId));

    this._activityService.remove({ activity: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick($event) {
    this._editActityDialog.create({ activityId: $event.data.activityId })
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.addOrUpdate(x)))
      .subscribe();
  }

  public handleFABButtonClick() {
    this._editActityDialog.create()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.addOrUpdate(x)))
      .subscribe();
  }

  public addOrUpdate(activity: Activity) {
    if (!activity) return;

    this.activities.update(activities => {
      const next = [...activities];
      const i = next.findIndex(t => t.activityId == activity.activityId);
      if (i < 0) {
        next.push(activity);
      } else {
        next[i] = activity;
      }
      return next;
    });
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
