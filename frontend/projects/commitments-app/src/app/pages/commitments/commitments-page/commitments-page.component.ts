// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { tap } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid-community';
import { CommitmentService } from '../../../services/commitment.service';
import { Commitment } from '../../../models/commitment';
import { EditCommitmentDialogService } from '../../../services/edit-commitment-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
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
  private readonly _destroyRef = inject(DestroyRef);

  public readonly commitments = signal<Commitment[]>([]);

  public localeText: any = {};

  ngOnInit() {
    this._commitmentService.getPersonal()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.commitments.set(x)))
      .subscribe();
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
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick($event) {
    this._editCommitmentDialog.create({ commitmentId: $event.data.commitmentId })
      .pipe(takeUntilDestroyed(this._destroyRef), tap(commitment => this.addOrUpdate(commitment)))
      .subscribe();
  }

  public handleRemoveClick($event) {
    const commitment = $event.data;

    this.commitments.update(commitments => commitments.filter(x => x.commitmentId != commitment.commitmentId));

    this._commitmentService.remove({ commitment })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public addOrUpdate(commitment: Commitment) {
    if (!commitment) return;

    this.commitments.update(commitments => {
      const next = [...commitments];
      const i = next.findIndex(t => t.commitmentId == commitment.commitmentId);
      if (i < 0) {
        next.push(commitment);
      } else {
        next[i] = commitment;
      }
      return next;
    });
  }
}
