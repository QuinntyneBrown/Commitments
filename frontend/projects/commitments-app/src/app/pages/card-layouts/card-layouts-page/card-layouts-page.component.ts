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
import { CardLayoutService } from '../../../services/card-layout.service';
import { CardLayout } from '../../../models/card-layout';
import { EditCardLayoutDialogService } from '../../../services/edit-card-layout-dialog.service';
import { CheckboxCellComponent } from '../../../components/checkbox-cell/checkbox-cell.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-card-layouts-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './card-layouts-page.component.html',
  styleUrls: ['./card-layouts-page.component.scss']
})
export class CardLayoutsPageComponent {
  private readonly _cardLayoutService = inject(CardLayoutService);
  private readonly _editCardLayoutDialog = inject(EditCardLayoutDialogService);
  private readonly _router = inject(Router);

  ngOnInit() {
    this._cardLayoutService.get()
      .pipe(map(x => this.cardLayouts$.next(x)))
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public cardLayouts$: BehaviorSubject<Array<CardLayout>> = new BehaviorSubject([]);

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleRemoveClick($event) {
    const cardLayouts: Array<CardLayout> = [...this.cardLayouts$.value];
    const index = cardLayouts.findIndex(x => x.cardLayoutId == $event.data.cardLayoutId);
    cardLayouts.splice(index, 1);
    this.cardLayouts$.next(cardLayouts);

    this._cardLayoutService.remove({ cardLayout: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleEditClick($event) {

  }

  public addOrUpdate(cardLayout: CardLayout) {
    if (!cardLayout) return;

    const cardLayouts = [...this.cardLayouts$.value];
    const i = cardLayouts.findIndex((t) => t.cardLayoutId == cardLayout.cardLayoutId);
    if (i < 0) {
      cardLayouts.push(cardLayout);
    } else {
      cardLayouts[i] = cardLayout;
    }
    this.cardLayouts$.next(cardLayouts);
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

  public handleFABButtonClick() {
    this._editCardLayoutDialog.create()
      .pipe(takeUntil(this.onDestroy), map(x => this.addOrUpdate(x)))
      .subscribe();
  }
}
