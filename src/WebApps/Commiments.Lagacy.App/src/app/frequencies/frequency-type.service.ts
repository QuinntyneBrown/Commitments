// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { FrequencyType } from "./frequency-type";

@Injectable()
export class FrequencyTypeService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<FrequencyType>> {
    return this._client.get<{ frequencyTypes: Array<FrequencyType> }>(`${this._baseUrl}api/frequencyTypes`)
      .pipe(
        map(x => x.frequencyTypes)
      );
  }

  public getById(options: { frequencyTypeId: number }): Observable<FrequencyType> {
    return this._client.get<{ frequencyType: FrequencyType }>(`${this._baseUrl}api/frequencyTypes/${options.frequencyTypeId}`)
      .pipe(
        map(x => x.frequencyType)
      );
  }

  public remove(options: { frequencyType: FrequencyType }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/frequencyTypes/${options.frequencyType.frequencyTypeId}`);
  }

  public save(options: { frequencyType: FrequencyType }): Observable<{ frequencyTypeId: number }> {
    return this._client.post<{ frequencyTypeId: number }>(`${this._baseUrl}api/frequencyTypes`, { frequencyType: options.frequencyType });
  }
}

