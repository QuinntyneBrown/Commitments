// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AsyncPipe, CommonModule, NgIf } from '@angular/common';
import { createLoginPageViewModel } from './create-login-page-view-model';
import { PushModule } from '@ngrx/component';
import { LoginComponent } from '@identity/core';

@Component({
  selector: 'app-login-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ 
    PushModule, 
    LoginComponent],
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  public vm$ = createLoginPageViewModel();
}
