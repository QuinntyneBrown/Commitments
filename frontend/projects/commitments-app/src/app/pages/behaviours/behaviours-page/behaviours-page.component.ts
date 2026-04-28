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
import { BehaviourService } from '../../../services/behaviour.service';
import { Behaviour } from '../../../models/behaviour';
import { BehaviourType } from '../../../models/behaviour-type';
import { BehaviourTypeService } from '../../../services/behaviour-type.service';
import { EditBehaviourDialogService } from '../../../services/edit-behaviour-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-edit-behaviour-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './behaviours-page.component.html',
  styleUrls: ['./behaviours-page.component.scss']
})
export class BehavioursPageComponent {
  private readonly _behaviourService = inject(BehaviourService);
  private readonly _behaviourTypeService = inject(BehaviourTypeService);
  private readonly _editBehaviourDialog = inject(EditBehaviourDialogService);
  private readonly _router = inject(Router);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly behaviour = signal<Behaviour>(<Behaviour>{});
  public readonly behaviours = signal<Array<Behaviour>>([]);
  public readonly behaviourTypes = signal<Array<BehaviourType>>([]);

  public localeText: any = {};

  ngOnInit() {
    this._behaviourService.get()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.behaviours.set(x)))
      .subscribe();
  }

  public handleFABButtonClick() {
    this._editBehaviourDialog.create({ behaviourId: this.behaviour().behaviourId })
      .pipe(takeUntilDestroyed(this._destroyRef), tap(b => this.addOrUpdate(b)))
      .subscribe();
  }

  public handleRemoveClick($event) {
    this.behaviours.update(bs => bs.filter(x => x.behaviourId != $event.data.behaviourId));

    this._behaviourService.remove({ behaviour: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick($event) {
    this._editBehaviourDialog.create({ behaviourId: $event.data.behaviourId })
      .pipe(takeUntilDestroyed(this._destroyRef), tap(b => this.addOrUpdate(b)))
      .subscribe();
  }

  public addOrUpdate(behaviour: Behaviour) {
    if (!behaviour) return;

    this.behaviours.update(bs => {
      const next = [...bs];
      const i = next.findIndex(t => t.behaviourId == behaviour.behaviourId);
      if (i < 0) {
        next.push(behaviour);
      } else {
        next[i] = behaviour;
      }
      return next;
    });
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Name", field: "name" },
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
