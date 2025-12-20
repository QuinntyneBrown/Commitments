// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { LanguageService } from '../../core/language.service';

@Component({
  templateUrl: './settings-page.html',
  styleUrls: ['./settings-page.scss'],
  selector: 'app-settings-page'
})
export class SettingsPage {
  constructor(private readonly _languageService: LanguageService) {}

  public get currentLanguage() {
    return this._languageService.current;
  }

  public set currentLanguage(value: string) {
    this._languageService.setCurrent(value);
  }
}
