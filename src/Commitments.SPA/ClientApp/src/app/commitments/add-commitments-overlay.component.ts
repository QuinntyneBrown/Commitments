import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { CommitmentService } from "./commitment.service";
import { Commitment } from "./commitment.model";
import { ColDef } from "ag-grid";
import { BehaviourService } from "../behaviours/behaviour.service";
import { Behaviour } from "../behaviours/behaviour.model";
import { FrequencyTypeService } from "./frequency-type.service";
import { FrequencyType } from "./frequency-type.model";
import { FormGroup, FormControl } from "@angular/forms";

@Component({
  templateUrl: "./add-commitments-overlay.component.html",
  styleUrls: ["./add-commitments-overlay.component.css"],
  selector: "app-add-commitments-overlay"
})
export class AddCommitmentsOverlayComponent { 
  constructor(
    private _overlay: OverlayRefWrapper,
    private _behaviourService: BehaviourService,
    private _commitmentService: CommitmentService,
    private _frequencyTypeService: FrequencyTypeService
  ) {
    
  }

  ngOnInit() {
    this.behaviours$ = this._behaviourService.get();
    this.frequencyTypes$ = this._frequencyTypeService.get();
    this.commitments$ = this._commitmentService.getByCurrentProfile();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick(behaviours) {    
    this._overlay.close(behaviours.selectedOptions.selected.map(el => el.value));
  }
  
  public commitments = [];

  public behaviours$: Observable<Array<Behaviour>>;
  public frequencyTypes$: Observable<Array<FrequencyType>>;
  public commitments$: Observable<Array<Commitment>>;

  public form: FormGroup = new FormGroup({
    frequency: new FormControl(null,[])
  });
}
