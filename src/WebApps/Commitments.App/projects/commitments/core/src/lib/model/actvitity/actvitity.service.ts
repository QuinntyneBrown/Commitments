// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { Actvitity } from './actvitity';

@Injectable({
  providedIn: 'root'
})
export class ActvitityService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Actvitity>> {
    return this._client.get<{ actvitities: Array<Actvitity> }>(`${this._baseUrl}api/1.0/actvitity`)
      .pipe(
        map(x => x.actvitities)
      );
  }

  public getById(options: { actvitityId: string }): Observable<Actvitity> {
    return this._client.get<{ actvitity: Actvitity }>(`${this._baseUrl}api/1.0/actvitity/${options.actvitityId}`)
      .pipe(
        map(x => x.actvitity)
      );
  }

  public delete(options: { actvitity: Actvitity }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/actvitity/${options.actvitity.actvitityId}`);
  }

  public create(options: { actvitity: Actvitity }): Observable<{ actvitityId: string  }> {    
    return this._client.post<{ actvitityId: string }>(`${this._baseUrl}api/1.0/actvitity`, { actvitity: options.actvitity });
  }

  public update(options: { actvitity: Actvitity }): Observable<{ actvitityId: string }> {    
    return this._client.post<{ actvitityId: string }>(`${this._baseUrl}api/1.0/actvitity`, { actvitity: options.actvitity });
  }
}
