// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatRadioModule } from '@angular/material/radio';
import { LanguageService } from '../../../core/language.service';
import { AuthService } from '../../../core/auth.service';
import { PrimaryHeaderComponent } from '@commitments/ui';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-settings-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatRadioModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.scss']
})
export class SettingsPageComponent {
  private readonly _languageService = inject(LanguageService);
  private readonly _authService = inject(AuthService);
  private readonly _router = inject(Router);

  public readonly version = environment.version;
  public readonly buildHash = environment.buildHash;

  public readonly accountLinks = [
    { label: 'My Profile', icon: 'person', route: '/my-profile' },
    { label: 'Profiles', icon: 'group', route: '/profiles' }
  ];

  public readonly catalogLinks = [
    { label: 'Behaviour Types', icon: 'category', route: '/behaviour-types' },
    { label: 'Behaviours', icon: 'psychology', route: '/behaviours' },
    { label: 'Frequencies', icon: 'schedule', route: '/frequencies' },
    { label: 'Cards', icon: 'dashboard', route: '/cards' },
    { label: 'Card Layouts', icon: 'view_quilt', route: '/card-layouts' }
  ];

  public get currentLanguage() {
    return this._languageService.current;
  }

  public set currentLanguage(value: string) {
    this._languageService.setCurrent(value);
  }

  public handleLogout() {
    this._authService.logout();
    this._router.navigate(['/login']);
  }
}
