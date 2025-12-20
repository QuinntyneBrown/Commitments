// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LocalStorageService } from '../core/local-storage.service';
import { map } from 'rxjs';
import { accessTokenKey, baseUrl } from '../core/constants';
import { HubClient } from '../core/hub-client';

@Injectable()
export class AuthService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl: string,
    private readonly _httpClient: HttpClient,
    private readonly _hubClient: HubClient,
    private readonly _localStorageService: LocalStorageService
  ) {}

  public logout() {
    this._hubClient.disconnect();
    this._localStorageService.put({ name: accessTokenKey, value: null });
  }

  public tryToLogin(options: { username: string; password: string }) {
    return this._httpClient.post<any>(`${this._baseUrl}api/users/token`, options).pipe(
      map(response => {
        this._localStorageService.put({ name: accessTokenKey, value: response.accessToken });
        return response.accessToken;
      })
    );
  }
}

