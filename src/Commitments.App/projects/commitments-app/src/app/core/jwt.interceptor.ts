// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { LoginRedirectService } from './redirect.service';
import { accessTokenKey } from './constants';
import { tap } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const localStorageService = inject(LocalStorageService);
  const loginRedirectService = inject(LoginRedirectService);

  return next(req).pipe(
    tap({
      error: (error) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          localStorageService.put({ name: accessTokenKey, value: null });
          loginRedirectService.redirectToLogin();
        }
      }
    })
  );
};
