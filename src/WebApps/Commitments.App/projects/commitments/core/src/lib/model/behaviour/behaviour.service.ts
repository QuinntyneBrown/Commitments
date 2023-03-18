// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { Behaviour } from './behaviour';

@Injectable({
  providedIn: 'root'
})
export class BehaviourService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Behaviour>> {
    return this._client.get<{ behaviours: Array<Behaviour> }>(`${this._baseUrl}api/1.0/behaviour`)
      .pipe(
        map(x => x.behaviours)
      );
  }

  public getById(options: { behaviourId: string }): Observable<Behaviour> {
    return this._client.get<{ behaviour: Behaviour }>(`${this._baseUrl}api/1.0/behaviour/${options.behaviourId}`)
      .pipe(
        map(x => x.behaviour)
      );
  }

  public delete(options: { behaviour: Behaviour }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/behaviour/${options.behaviour.behaviourId}`);
  }

  public create(options: { behaviour: Behaviour }): Observable<{ behaviourId: string  }> {    
    return this._client.post<{ behaviourId: string }>(`${this._baseUrl}api/1.0/behaviour`, { behaviour: options.behaviour });
  }

  public update(options: { behaviour: Behaviour }): Observable<{ behaviourId: string }> {    
    return this._client.post<{ behaviourId: string }>(`${this._baseUrl}api/1.0/behaviour`, { behaviour: options.behaviour });
  }
}
