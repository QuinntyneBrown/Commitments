// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { Card } from "./card";

@Injectable()
export class CardService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Card>> {
    return this._client.get<{ cards: Array<Card> }>(`${this._baseUrl}api/cards`)
      .pipe(
        map(x => x.cards)
      );
  }

  public getById(options: { cardId: number }): Observable<Card> {
    return this._client.get<{ card: Card }>(`${this._baseUrl}api/cards/${options.cardId}`)
      .pipe(
        map(x => x.card)
      );
  }

  public remove(options: { card: Card }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/cards/${options.card.cardId}`);
  }

  public save(options: { card: Card }): Observable<{ cardId: number }> {
    return this._client.post<{ cardId: number }>(`${this._baseUrl}api/cards`, { card: options.card });
  }
}

