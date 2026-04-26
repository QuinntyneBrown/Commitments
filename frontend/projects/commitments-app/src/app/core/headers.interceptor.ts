// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey } from './constants';

export const headerInterceptor: HttpInterceptorFn = (req, next) => {
  const storage = inject(LocalStorageService);
  const token = storage.get({ name: accessTokenKey }) || '';

  const clonedReq = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`)
  });

  return next(clonedReq);
};
