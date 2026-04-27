// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AgGridModule } from 'ag-grid-angular';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, switchMap, takeUntil } from 'rxjs';
import { ColDef, GridApi } from 'ag-grid';
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

  ngOnInit() {
    this._cardService.get()
      .pipe(
        map(cards => this.cards$.next(cards)),
        switchMap(() => this.cards$),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public handleFABButtonClick() {
    this._editCardDialog.create()
      .pipe(
        map(x => this.addOrUpdate(x)),
        takeUntil(this.onDestroy)
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

  public cards$: BehaviorSubject<Card[]> = new BehaviorSubject([]);

  public handleEditClick($event) {
    const overlayRefWrapper = this._editCardDialog
      .create({ cardId: $event.data.cardId })
      .pipe(map(card => this.addOrUpdate(card)), takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleRemove($event) {
    const card = $event.data;

    const cards: Array<Card> = [...this.cards$.value];
    const index = cards.findIndex(x => x.cardId == $event.data.cardId);
    cards.splice(index, 1);
    this.cards$.next(cards);

    this._cardService.remove({ card: card })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public addOrUpdate(card: Card) {
    if (!card) return;

    let cards = [...this.cards$.value];
    const i = cards.findIndex((t) => t.cardId == card.cardId);

    if (i < 0) {
      cards.push(card);
    } else {
      cards[i] = card;
    }

    this.cards$.next(cards);
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
