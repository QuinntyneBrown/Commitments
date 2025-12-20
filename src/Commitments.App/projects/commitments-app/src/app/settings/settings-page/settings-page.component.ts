// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { LocalStorageService } from '../../core/local-storage.service';
import { accessTokenKey } from '../../core/constants';
import { LanguageService } from '../../core/language.service';

@Component({
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.scss'],
  selector: 'app-settings-page'
})
export class SettingsPageComponent {
  constructor(private readonly _languageService: LanguageService) {}

  public get currentLanguage() {
    return this._languageService.current;
  }

  public set currentLanguage(value: string) {
    this._languageService.setCurrent(value);
  }
}

