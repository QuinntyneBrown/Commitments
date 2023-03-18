// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { BehaviourType } from "./behaviour-type";

@Injectable()
export class BehaviourTypeService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<BehaviourType>> {
    return this._client.get<{ behaviourTypes: Array<BehaviourType> }>(`${this._baseUrl}api/behaviourTypes`)
      .pipe(
        map(x => x.behaviourTypes)
      );
  }

  public getById(options: { behaviourTypeId: number }): Observable<BehaviourType> {
    return this._client.get<{ behaviourType: BehaviourType }>(`${this._baseUrl}api/behaviourTypes/${options.behaviourTypeId}`)
      .pipe(
        map(x => x.behaviourType)
      );
  }

  public remove(options: { behaviourType: BehaviourType }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/behaviourTypes/${options.behaviourType.behaviourTypeId}`);
  }

  public save(options: { behaviourType: BehaviourType }): Observable<{ behaviourTypeId: number }> {
    return this._client.post<{ behaviourTypeId: number }>(`${this._baseUrl}api/behaviourTypes`, { behaviourType: options.behaviourType });
  }
}

