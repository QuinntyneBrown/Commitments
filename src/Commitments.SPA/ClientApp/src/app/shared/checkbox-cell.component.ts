import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ICellRendererAngularComp } from "ag-grid-angular";
import { ICellRendererParams, IAfterGuiAttachedParams } from "ag-grid";

@Component({
  templateUrl: "./checkbox-cell.component.html",
  styleUrls: ["./checkbox-cell.component.css"],
  selector: "app-checkbox-cell"
})
export class CheckboxCellComponent implements ICellRendererAngularComp { 

  public params: ICellRendererParams;

  refresh(params: any): boolean {
    return true;
  }

  agInit(params: ICellRendererParams): void {    
    this.params = params;
  }

  onChange($event) {
    this.params.value = $event;
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
