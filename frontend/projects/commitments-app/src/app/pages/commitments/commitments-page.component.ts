// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid';
import { CommitmentService } from '../../services/commitment.service';
import { Commitment } from '../../models/commitment';
import { EditCommitmentDialogService } from '../../services/edit-commitment-dialog.service';
import { CheckboxCellComponent } from '../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-commitments-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './commitments-page.component.html',
  styleUrls: ['./commitments-page.component.scss']
})
export class CommitmentsPageComponent {
  private readonly _commitmentService = inject(CommitmentService);
  private readonly _editCommitmentDialog = inject(EditCommitmentDialogService);

  ngOnInit() {
    this._commitmentService.getPersonal()
      .pipe(takeUntil(this.onDestroy), map(x => this.commitments$.next(x)))
      .subscribe();
  }

  public commitments$: BehaviorSubject<Commitment[]> = new BehaviorSubject([]);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Type", field: "behaviour.behaviourType.name" },
    { headerName: "Name", field: "behaviour.name" },
    { cellRenderer: "editRenderer", onCellClicked: $event => this.handleEditClick($event), width: 50 },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemoveClick($event), width: 50 }
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

  public handleFABButtonClick() {
    this._editCommitmentDialog.create()
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditClick($event) {
    this._editCommitmentDialog.create({ commitmentId: $event.data.commitmentId })
      .pipe(map(commitment => this.addOrUpdate(commitment)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleRemoveClick($event) {
    const commitment = $event.data;

    const commitments: Array<Commitment> = [...this.commitments$.value];
    const index = commitments.findIndex(x => x.commitmentId == $event.data.commitmentId);
    commitments.splice(index, 1);
    this.commitments$.next(commitments);

    this._commitmentService.remove({ commitment })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public addOrUpdate(commitment: Commitment) {
    if (!commitment) return;

    let commitments = [...this.commitments$.value];
    const i = commitments.findIndex((t) => t.commitmentId == commitment.commitmentId);

    if (i < 0) {
      commitments.push(commitment);
    } else {
      commitments[i] = commitment;
    }

    this.commitments$.next(commitments);
  }
}
