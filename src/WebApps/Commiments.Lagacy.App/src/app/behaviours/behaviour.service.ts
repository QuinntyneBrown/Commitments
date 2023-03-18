// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { Behaviour } from "./behaviour";
import { baseUrl } from "../core/constants";

@Injectable()
export class BehaviourService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<Behaviour>> {
    return this._client.get<{ behaviours: Array<Behaviour> }>(`${this._baseUrl}api/behaviours`)
      .pipe(
        map(x => x.behaviours)
      );
  }

  public getById(options: { behaviourId: number }): Observable<Behaviour> {
    return this._client.get<{ behaviour: Behaviour }>(`${this._baseUrl}api/behaviours/${options.behaviourId}`)
      .pipe(
        map(x => x.behaviour)
      );
  }

  public remove(options: { behaviour: Behaviour }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/behaviours/${options.behaviour.behaviourId}`);
  }

  public save(options: { behaviour: Behaviour }): Observable<{ behaviourId: number }> {    
    return this._client.post<{ behaviourId: number }>(`${this._baseUrl}api/behaviours`, { behaviour: options.behaviour });
  }
}

