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
import { CardService } from '../../../services/card.service';
import { Card } from '../../../models/card';
import { EditCardDialogService } from '../../../services/edit-card-dialog.service';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { EditCellComponent } from '../../../components/edit-cell/edit-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-cards-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './cards-page.component.html',
  styleUrls: ['./cards-page.component.scss']
})
export class CardsPageComponent {
  private readonly _cardService = inject(CardService);
  private readonly _editCardDialog = inject(EditCardDialogService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly cards = signal<Card[]>([]);

  public localeText: any = {};

  ngOnInit() {
    this._cardService.get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(cards => this.cards.set(cards))
      )
      .subscribe();
  }

  public handleFABButtonClick() {
    this._editCardDialog.create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => this.addOrUpdate(x))
      )
      .subscribe();
  }

  public columnDefs: Array<ColDef> = [
    { headerName: "Name", field: "name" },
    { cellRenderer: "editRenderer", onCellClicked: $event => this.handleEditClick($event), width: 30 },
    { cellRenderer: "deleteRenderer", onCellClicked: $event => this.handleRemove($event), width: 30 }
  ];

  public frameworkComponents: any = {
    deleteRenderer: DeleteCellComponent,
    editRenderer: EditCellComponent
  };

  private _gridApi: GridApi;

  public onGridReady(params) {
    this._gridApi = params.api;
    this._gridApi.sizeColumnsToFit();
  }

  public handleEditClick($event) {
    this._editCardDialog
      .create({ cardId: $event.data.cardId })
      .pipe(takeUntilDestroyed(this._destroyRef), tap(card => this.addOrUpdate(card)))
      .subscribe();
  }

  public handleRemove($event) {
    const card = $event.data;

    this.cards.update(cards => cards.filter(x => x.cardId != card.cardId));

    this._cardService.remove({ card })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public addOrUpdate(card: Card) {
    if (!card) return;

    this.cards.update(cards => {
      const next = [...cards];
      const i = next.findIndex(t => t.cardId == card.cardId);
      if (i < 0) {
        next.push(card);
      } else {
        next[i] = card;
      }
      return next;
    });
  }
}
