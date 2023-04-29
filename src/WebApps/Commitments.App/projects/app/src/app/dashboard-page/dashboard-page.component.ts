// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { createDashboardPageViewModel } from './create-dashboard-page-view-model';
import { PushModule } from '@ngrx/component';
import { Dialog, DialogModule } from '@angular/cdk/dialog';
import { CardStore, SelectCardComponent } from '@dashboard/core';


@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, PushModule, DialogModule],
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss']
})
export class DashboardPageComponent implements OnInit {

  private readonly _dialog = inject(Dialog);

  private readonly _cardStore = inject(CardStore);

  public vm$ = createDashboardPageViewModel();

  ngOnInit() {
    this._cardStore.load();    
  }
}
