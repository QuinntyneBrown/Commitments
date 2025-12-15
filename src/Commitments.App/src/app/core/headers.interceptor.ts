// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable } from '@angular/core';
import { accessTokenKey, storageKey } from './constants';
import { LocalStorageService } from './local-storage.service';
import { Observable } from 'rxjs';
import {
  HttpClient,
  HttpEvent,
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
} from '@angular/common/http';

@Injectable()
export class HeaderInterceptor implements HttpInterceptor {
  constructor(private readonly _storage: LocalStorageService) {}

  intercept(
    httpRequest: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = this._storage.get({ name: accessTokenKey }) || '';

    return next.handle(
      httpRequest.clone({
        headers: httpRequest.headers.set('Authorization', `Bearer ${token}`),
      })
    );
  }
}

