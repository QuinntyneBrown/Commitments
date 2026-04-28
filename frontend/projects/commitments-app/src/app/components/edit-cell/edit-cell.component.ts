// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-edit-cell',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './edit-cell.component.html',
  styleUrls: ['./edit-cell.component.scss']
})
export class EditCellComponent implements ICellRendererAngularComp {
  refresh(_params: any): boolean {
    return true;
  }

  agInit(_params: ICellRendererParams): void {}
}
