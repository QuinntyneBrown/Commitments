// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ICellRendererAngularComp } from "ag-grid-angular";
import { ICellRendererParams } from "ag-grid";

@Component({
  templateUrl: "./checkbox-cell.html",
  styleUrls: ["./checkbox-cell.scss"],
  selector: "app-checkbox-cell"
})
export class CheckboxCell implements ICellRendererAngularComp {

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
