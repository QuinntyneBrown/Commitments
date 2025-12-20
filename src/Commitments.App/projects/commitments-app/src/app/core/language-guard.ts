// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HubClient } from './hub-client';
import { Observable } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { map } from 'rxjs';
import { LanguageService } from './language.service';

@Injectable()
export class LanguageGuard implements CanActivate {
  constructor(
    private readonly _languageService: LanguageService,
    private readonly _translationService: TranslateService
  ) {}

  public canActivate(): Observable<boolean> {
    return this._translationService.get(['Compose a note...']).pipe(
      map(translations => {
        this._languageService.currentTranslations = translations;
        return true;
      })
    );
  }
}

