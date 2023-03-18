// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { Commitment } from './commitment';

@Injectable({
  providedIn: 'root'
})
export class CommitmentService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Commitment>> {
    return this._client.get<{ commitments: Array<Commitment> }>(`${this._baseUrl}api/1.0/commitment`)
      .pipe(
        map(x => x.commitments)
      );
  }

  public getById(options: { commitmentId: string }): Observable<Commitment> {
    return this._client.get<{ commitment: Commitment }>(`${this._baseUrl}api/1.0/commitment/${options.commitmentId}`)
      .pipe(
        map(x => x.commitment)
      );
  }

  public delete(options: { commitment: Commitment }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/commitment/${options.commitment.commitmentId}`);
  }

  public create(options: { commitment: Commitment }): Observable<{ commitmentId: string  }> {    
    return this._client.post<{ commitmentId: string }>(`${this._baseUrl}api/1.0/commitment`, { commitment: options.commitment });
  }

  public update(options: { commitment: Commitment }): Observable<{ commitmentId: string }> {    
    return this._client.post<{ commitmentId: string }>(`${this._baseUrl}api/1.0/commitment`, { commitment: options.commitment });
  }
}
