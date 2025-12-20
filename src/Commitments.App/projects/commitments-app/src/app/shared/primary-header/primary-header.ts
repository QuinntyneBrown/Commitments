// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  templateUrl: './primary-header.html',
  styleUrls: ['./primary-header.scss'],
  selector: 'app-primary-header'
})
export class PrimaryHeader {
  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
