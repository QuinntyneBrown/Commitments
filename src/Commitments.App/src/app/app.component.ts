// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from './core/language.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(
    private readonly _languageService: LanguageService,
    private readonly _translateService: TranslateService
  ) {
    _translateService.setDefaultLang(_languageService.default);

    _translateService.use(_languageService.current);
  }
}

