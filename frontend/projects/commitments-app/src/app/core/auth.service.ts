// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';
import { accessTokenKey, baseUrl, currentProfileIdKey } from './constants';
import { HubClient } from './hub-client';
import { LocalStorageService } from './local-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _baseUrl = inject(baseUrl as any) as string;
  private readonly _httpClient = inject(HttpClient);
  private readonly _hubClient = inject(HubClient);
  private readonly _localStorageService = inject(LocalStorageService);

  public logout() {
    this._hubClient.disconnect();
    this._localStorageService.put({ name: accessTokenKey, value: null });
    this._localStorageService.put({ name: currentProfileIdKey, value: null });
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
