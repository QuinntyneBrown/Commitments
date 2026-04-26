// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { IAfterGuiAttachedParams, ICellRendererParams } from 'ag-grid';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-delete-cell',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './delete-cell.component.html',
  styleUrls: ['./delete-cell.component.scss']
})
export class DeleteCellComponent implements ICellRendererAngularComp {
  refresh(params: any): boolean {
    return true;
  }

  agInit(params: ICellRendererParams): void {}

  afterGuiAttached?(params?: IAfterGuiAttachedParams): void {}

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
