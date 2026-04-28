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
  private readonly _destroyRef = inject(DestroyRef);

  public readonly cardLayouts = signal<Array<CardLayout>>([]);

  public localeText: any = {};

  ngOnInit() {
    this._cardLayoutService.get()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.cardLayouts.set(x)))
      .subscribe();
  }

  public handleRemoveClick($event) {
    this.cardLayouts.update(layouts => layouts.filter(x => x.cardLayoutId != $event.data.cardLayoutId));

    this._cardLayoutService.remove({ cardLayout: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick($event) {

  }

  public addOrUpdate(cardLayout: CardLayout) {
    if (!cardLayout) return;

    this.cardLayouts.update(layouts => {
      const next = [...layouts];
      const i = next.findIndex(t => t.cardLayoutId == cardLayout.cardLayoutId);
      if (i < 0) {
        next.push(cardLayout);
      } else {
        next[i] = cardLayout;
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

  public handleFABButtonClick() {
    this._editCardLayoutDialog.create()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this.addOrUpdate(x)))
      .subscribe();
  }
}
