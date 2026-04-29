// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  return next(req).pipe(
    tap({ error: (e) => {
      if (e instanceof HttpErrorResponse && e.status === 401) {
        localStorage.removeItem('accessTokenKey');
        router.navigate(['/login']);
      }
    }})
  );
};
