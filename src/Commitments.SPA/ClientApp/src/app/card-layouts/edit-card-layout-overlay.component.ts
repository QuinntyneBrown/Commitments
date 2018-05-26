import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { CardLayoutService } from "./card-layout.service";
import { CardLayout } from "./card-layout.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./edit-card-layout-overlay.component.html",
  styleUrls: ["./edit-card-layout-overlay.component.css"],
  selector: "app-edit-card-layout-overlay"
})
export class EditCardLayoutOverlayComponent { 
  constructor(
    private _cardLayoutService: CardLayoutService,
    private _overlay: OverlayRefWrapper) { }

  ngOnInit() {
    if (this.cardLayoutId)
      this._cardLayoutService.getById({ cardLayoutId: this.cardLayoutId })
        .pipe(
          map(x => this.cardLayout$.next(x)),
          switchMap(x => this.cardLayout$),
          map(x => this.form.patchValue({
            name: x.name
          }))
        )
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public cardLayout$: BehaviorSubject<CardLayout> = new BehaviorSubject(<CardLayout>{});
  
  public cardLayoutId: number;

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const cardLayout = new CardLayout();
    cardLayout.cardLayoutId = this.cardLayoutId;
    cardLayout.name = this.form.value.name;
    this._cardLayoutService.save({ cardLayout })
      .pipe(
        map(x => cardLayout.cardLayoutId = x.cardLayoutId),
        tap(x => this._overlay.close(cardLayout)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [])
  });
} 
