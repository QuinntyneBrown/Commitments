import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { CommitmentService } from "./commitment.service";
import { Commitment } from "./commitment.model";
import { ColDef } from "ag-grid";

@Component({
  templateUrl: "./add-commitments-overlay.component.html",
  styleUrls: ["./add-commitments-overlay.component.css"],
  selector: "app-add-commitments-overlay"
})
export class AddCommitmentsOverlayComponent { 
  constructor(
    private _overlay: OverlayRefWrapper,
    private _commitmentsService: CommitmentService
  ) { }

  ngOnInit() {
    this.commitments$ = this._commitmentsService.get();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public frameworkComponents = {

  };
  
  public handleSaveClick(commitments) {    
    this._overlay.close(commitments.selectedOptions.selected.map(el => el.value));
  }

  public onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public commitments = [];

  public commitments$: Observable<Array<Commitment>>;

  public columnDefs: Array<ColDef> = [
    {
      headerName: 'Name',
      field: 'name'
    }
  ];
}
