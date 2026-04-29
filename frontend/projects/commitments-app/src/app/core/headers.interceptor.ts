// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpInterceptorFn } from '@angular/common/http';

export const headerInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('accessTokenKey') || '';
  return next(req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) }));
};
