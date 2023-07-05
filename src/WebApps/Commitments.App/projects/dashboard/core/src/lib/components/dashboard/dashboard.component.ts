// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { createDashboardViewModel } from './create-dashboard-view-model';
import { PushModule } from '@ngrx/component';
import { PortalModule } from '@angular/cdk/portal';

@Component({
  selector: 'db-dashboard',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, PushModule, PortalModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  public vm$ = createDashboardViewModel();
}
