// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { DashboardShellComponent } from '@commitments/dashboard-framework';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [DashboardShellComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {}
