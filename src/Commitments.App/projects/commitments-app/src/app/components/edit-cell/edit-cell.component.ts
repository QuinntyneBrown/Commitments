// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-edit-cell',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './edit-cell.component.html',
  styleUrls: ['./edit-cell.component.scss']
})
export class EditCellComponent implements ICellRendererAngularComp {
  refresh(params: any): boolean {
    return true;
  }

  agInit(params: ICellRendererParams): void {}

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
