// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { IAfterGuiAttachedParams, ICellRendererParams } from 'ag-grid';

@Component({
  templateUrl: './star-cell.component.html',
  styles: [':host { margin: 0 auto; }'],
  selector: 'app-star-cell'
})
export class StarCellComponent implements ICellRendererAngularComp {
  refresh(params: any): boolean {
    return true;
  }

  agInit(params: ICellRendererParams): void { }

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void { }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

