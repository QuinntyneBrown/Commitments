// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ICellRendererAngularComp } from "ag-grid-angular";
import { ICellRendererParams } from "ag-grid";

@Component({
  templateUrl: "./edit-cell.component.html",
  styleUrls: ["./edit-cell.component.scss"],
  selector: "app-edit-cell"
})
export class EditCellComponent implements ICellRendererAngularComp { 
  refresh(params: any): boolean {
    return true;
  }

  agInit(params: ICellRendererParams): void { }

  
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

