// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { CardLayout } from "./card-layout";

@Injectable()
export class CardLayoutService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<CardLayout>> {
    return this._client.get<{ cardLayouts: Array<CardLayout> }>(`${this._baseUrl}api/cardLayouts`)
      .pipe(
        map(x => x.cardLayouts)
      );
  }

  public getById(options: { cardLayoutId: number }): Observable<CardLayout> {
    return this._client.get<{ cardLayout: CardLayout }>(`${this._baseUrl}api/cardLayouts/${options.cardLayoutId}`)
      .pipe(
        map(x => x.cardLayout)
      );
  }

  public remove(options: { cardLayout: CardLayout }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/cardLayouts/${options.cardLayout.cardLayoutId}`);
  }

  public save(options: { cardLayout: CardLayout }): Observable<{ cardLayoutId: number }> {
    return this._client.post<{ cardLayoutId: number }>(`${this._baseUrl}api/cardLayouts`, { cardLayout: options.cardLayout });
  }
}

