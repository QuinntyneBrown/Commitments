// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { createAppViewModel } from './create-app-view-model';
import { DialogModule } from '@angular/cdk/dialog';
import { PushModule } from '@ngrx/component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    DialogModule,
    PushModule
  ]
})
export class AppComponent {
  public vm$ = createAppViewModel();
}
