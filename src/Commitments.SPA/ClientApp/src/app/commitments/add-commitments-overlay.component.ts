import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { CommitmentService } from "./commitment.service";
import { Commitment } from "./commitment.model";
import { ColDef } from "ag-grid";
import { BehaviourService } from "../behaviours/behaviour.service";
import { Behaviour } from "../behaviours/behaviour.model";

@Component({
  templateUrl: "./add-commitments-overlay.component.html",
  styleUrls: ["./add-commitments-overlay.component.css"],
  selector: "app-add-commitments-overlay"
})
export class AddCommitmentsOverlayComponent { 
  constructor(
    private _overlay: OverlayRefWrapper,
    private _behaviourService: BehaviourService,
    private _commitmentsService: CommitmentService
  ) {
    this.behaviours$ = this._behaviourService.get();
  }

  ngOnInit() {
    
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
  
}
