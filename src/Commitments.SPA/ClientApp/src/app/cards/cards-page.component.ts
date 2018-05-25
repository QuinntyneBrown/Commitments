import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { EditCardOverlay } from "./edit-card-overlay";
import { CardService } from "./card.service";
import { Card } from "./card.model";
import { switchMap, takeUntil, map } from "rxjs/operators";
import { GridApi, ColDef } from "ag-grid";
import { DeleteCellComponent } from "../shared/delete-cell.component";
import { EditCellComponent } from "../shared/edit-cell.component";

@Component({
  templateUrl: "./cards-page.component.html",
  styleUrls: ["./cards-page.component.css"],
  selector: "app-cards-page"
})
export class CardsPageComponent { 
  constructor(
    private _cardService: CardService,
    private _editCardOverlay: EditCardOverlay) {

  }

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
    this._editCardOverlay.create()
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
    const overlayRefWrapper = this._editCardOverlay
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
