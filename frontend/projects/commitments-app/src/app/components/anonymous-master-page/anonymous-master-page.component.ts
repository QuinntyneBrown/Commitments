// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-anonymous-master-page',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './anonymous-master-page.component.html',
  styleUrls: ['./anonymous-master-page.component.scss']
})
export class AnonymousMasterPageComponent {}
