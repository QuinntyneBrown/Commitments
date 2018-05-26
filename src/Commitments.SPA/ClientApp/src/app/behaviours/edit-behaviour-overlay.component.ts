import { Component } from "@angular/core";
import { Subject, BehaviorSubject, Observable } from "rxjs";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { BehaviourService } from "./behaviour.service";
import { Behaviour } from "./behaviour.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";
import { BehaviourType } from "./behaviour-type.model";
import { BehaviourTypeService } from "./behaviour-type.service";

@Component({
  templateUrl: "./edit-behaviour-overlay.component.html",
  styleUrls: ["./edit-behaviour-overlay.component.css"],
  selector: "app-edit-behaviour-overlay"
})
export class EditBehaviourOverlayComponent { 
  constructor(
    private _behaviourService: BehaviourService,
    private _behaviourTypeService: BehaviourTypeService,
    private _overlay: OverlayRefWrapper) { }

  public behaviourTypes$: Observable<BehaviourType[]>;

  ngOnInit() {
    this.behaviourTypes$ = this._behaviourTypeService.get();

    if (this.behaviourId)
      this._behaviourService.getById({ behaviourId: this.behaviourId })
        .pipe(
          map(x => this.behaviour$.next(x)),
          switchMap(x => this.behaviour$),
          map(x => this.form.patchValue({
            name: x.name,
            description: x.description,
            behaviourTypeId: x.behaviourTypeId,
            isDesired: x.isDesired
          }))
        )
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() { this.onDestroy.next(); }

  public behaviour$: BehaviorSubject<Behaviour> = new BehaviorSubject(<Behaviour>{});
  
  public behaviourId: number;

  public handleCancelClick() { this._overlay.close(); }

  public handleSaveClick() {
    var behaviour = new Behaviour();
    behaviour.isDesired = this.form.value.isDesired;
    behaviour.behaviourTypeId = this.form.value.behaviourTypeId;
    behaviour.name = this.form.value.name;
    behaviour.description = this.form.value.description;

    this._behaviourService.save({ behaviour })
      .pipe(
        map(x => behaviour.behaviourId = x.behaviourId),
        tap(x => this._overlay.close(behaviour)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    isDesired: new FormControl(true, [Validators.required]),
    behaviourTypeId: new FormControl(null, [Validators.required])
  });
} 
