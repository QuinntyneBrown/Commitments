// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { BehaviourType } from './behaviour-type';

@Injectable({
  providedIn: 'root'
})
export class BehaviourTypeService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<BehaviourType>> {
    return this._client.get<{ behaviourTypes: Array<BehaviourType> }>(`${this._baseUrl}api/1.0/behaviourType`)
      .pipe(
        map(x => x.behaviourTypes)
      );
  }

  public getById(options: { behaviourTypeId: string }): Observable<BehaviourType> {
    return this._client.get<{ behaviourType: BehaviourType }>(`${this._baseUrl}api/1.0/behaviourType/${options.behaviourTypeId}`)
      .pipe(
        map(x => x.behaviourType)
      );
  }

  public delete(options: { behaviourType: BehaviourType }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/behaviourType/${options.behaviourType.behaviourTypeId}`);
  }

  public create(options: { behaviourType: BehaviourType }): Observable<{ behaviourTypeId: string  }> {    
    return this._client.post<{ behaviourTypeId: string }>(`${this._baseUrl}api/1.0/behaviourType`, { behaviourType: options.behaviourType });
  }

  public update(options: { behaviourType: BehaviourType }): Observable<{ behaviourTypeId: string }> {    
    return this._client.post<{ behaviourTypeId: string }>(`${this._baseUrl}api/1.0/behaviourType`, { behaviourType: options.behaviourType });
  }
}
