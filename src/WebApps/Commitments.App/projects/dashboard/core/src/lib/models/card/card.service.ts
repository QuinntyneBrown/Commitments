// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../../constants';
import { map, Observable } from 'rxjs';
import { Card } from './card';

@Injectable({
  providedIn: 'root'
})
export class CardService {

  constructor(
    @Inject(BASE_URL) private readonly _baseUrl: string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Card>> {
    return this._client.get<{ cards: Array<Card> }>(`${this._baseUrl}api/1.0/card`)
      .pipe(
        map(x => x.cards)
      );
  }

  public getById(options: { cardId: string }): Observable<Card> {
    return this._client.get<{ card: Card }>(`${this._baseUrl}api/1.0/card/${options.cardId}`)
      .pipe(
        map(x => x.card)
      );
  }

  public delete(options: { card: Card }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/1.0/card/${options.card.cardId}`);
  }

  public create(options: { card: Card }): Observable<{ cardId: string  }> {    
    return this._client.post<{ cardId: string }>(`${this._baseUrl}api/1.0/card`, { card: options.card });
  }

  public update(options: { card: Card }): Observable<{ cardId: string }> {    
    return this._client.post<{ cardId: string }>(`${this._baseUrl}api/1.0/card`, { card: options.card });
  }
}
