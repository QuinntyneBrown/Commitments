// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { map } from 'rxjs';
import { LanguageService } from './language.service';

export const languageGuard: CanActivateFn = () => {
  const languageService = inject(LanguageService);
  const translationService = inject(TranslateService);

  return translationService.get(['Compose a note...']).pipe(
    map(translations => {
      languageService.currentTranslations = translations;
      return true;
    })
  );
};
