import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { takeUntil, tap } from "rxjs/operators";
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from "@angular/router";
import { BehaviourService } from "./behaviour.service";
import { Behaviour } from "./behaviour.model";
import { BehaviourTypeService } from "./behaviour-type.service";

import { BehaviourType } from "./behaviour-type.model";

@Component({
  templateUrl: "./edit-behaviour-page.component.html",
  styleUrls: ["./edit-behaviour-page.component.css"],
  selector: "app-edit-behaviour-page"
})
export class EditBehaviourPageComponent { 
  constructor(
    private _behaviourService: BehaviourService,
    private _behaviourTypeService: BehaviourTypeService,
    private _router: Router
  ) { }

  ngOnInit() {
    this.behaviourTypes$ = this._behaviourTypeService.get();
  }

  public behaviourTypes$: Observable<Array<BehaviourType>>;

  public onDestroy: Subject<void> = new Subject<void>();

  public behaviour: Behaviour;
  
  public handleSaveClick() {

    var entity = new Behaviour();
    entity.isDesired = this.form.value.isDesired;
    entity.behaviourTypeId = this.form.value.behaviourTypeId;
    entity.name = this.form.value.name;
    entity.description = this.form.value.description;

    this._behaviourService.save({ behaviour: entity })
      .pipe(takeUntil(this.onDestroy))
      .subscribe(() => this._router.navigateByUrl("/"));
  }

  ngOnDestroy() {
    this.onDestroy.next();    
  }

  public form = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    isDesired: new FormControl(true, [Validators.required]),
    behaviourTypeId: new FormControl(null,[Validators.required])
  });

}
