// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { FrequencyType } from './frequency-type';

@Injectable({
  providedIn: 'root'
})
export class FrequencyTypeService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<FrequencyType>> {
    return this._client.get<{ frequencyTypes: Array<FrequencyType> }>(`${this._baseUrl}api/1.0/frequencyType`)
      .pipe(
        map(x => x.frequencyTypes)
      );
  }

  public getById(options: { frequencyTypeId: string }): Observable<FrequencyType> {
    return this._client.get<{ frequencyType: FrequencyType }>(`${this._baseUrl}api/1.0/frequencyType/${options.frequencyTypeId}`)
      .pipe(
        map(x => x.frequencyType)
      );
  }

  public delete(options: { frequencyType: FrequencyType }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/frequencyType/${options.frequencyType.frequencyTypeId}`);
  }

  public create(options: { frequencyType: FrequencyType }): Observable<{ frequencyTypeId: string  }> {    
    return this._client.post<{ frequencyTypeId: string }>(`${this._baseUrl}api/1.0/frequencyType`, { frequencyType: options.frequencyType });
  }

  public update(options: { frequencyType: FrequencyType }): Observable<{ frequencyTypeId: string }> {    
    return this._client.post<{ frequencyTypeId: string }>(`${this._baseUrl}api/1.0/frequencyType`, { frequencyType: options.frequencyType });
  }
}
