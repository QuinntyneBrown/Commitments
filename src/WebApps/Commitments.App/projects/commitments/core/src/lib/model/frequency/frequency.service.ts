// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { Frequency } from './frequency';

@Injectable({
  providedIn: 'root'
})
export class FrequencyService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Frequency>> {
    return this._client.get<{ frequencies: Array<Frequency> }>(`${this._baseUrl}api/1.0/frequency`)
      .pipe(
        map(x => x.frequencies)
      );
  }

  public getById(options: { frequencyId: string }): Observable<Frequency> {
    return this._client.get<{ frequency: Frequency }>(`${this._baseUrl}api/1.0/frequency/${options.frequencyId}`)
      .pipe(
        map(x => x.frequency)
      );
  }

  public delete(options: { frequency: Frequency }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/frequency/${options.frequency.frequencyId}`);
  }

  public create(options: { frequency: Frequency }): Observable<{ frequencyId: string  }> {    
    return this._client.post<{ frequencyId: string }>(`${this._baseUrl}api/1.0/frequency`, { frequency: options.frequency });
  }

  public update(options: { frequency: Frequency }): Observable<{ frequencyId: string }> {    
    return this._client.post<{ frequencyId: string }>(`${this._baseUrl}api/1.0/frequency`, { frequency: options.frequency });
  }
}
