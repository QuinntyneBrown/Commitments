// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs";
import { baseUrl } from "../core/constants";
import { ToDo } from "./to-do";

@Injectable()
export class ToDoService {
  constructor(
    @Inject(baseUrl) private readonly _baseUrl:string,
    private readonly _client: HttpClient
  ) { }

  public get(): Observable<Array<ToDo>> {
    return this._client.get<{ toDos: Array<ToDo> }>(`${this._baseUrl}api/toDos`)
      .pipe(
        map(x => x.toDos)
      );
  }

  public getOutstandingToDos(): Observable<Array<ToDo>> {
    return this._client.get<{ toDos: Array<ToDo> }>(`${this._baseUrl}api/toDos/outstanding`)
      .pipe(
        map(x => x.toDos)
      );
  }

  public getById(options: { toDoId: number }): Observable<ToDo> {
    return this._client.get<{ toDo: ToDo }>(`${this._baseUrl}api/toDos/${options.toDoId}`)
      .pipe(
        map(x => x.toDo)
      );
  }

  public remove(options: { toDo: ToDo }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/toDos/${options.toDo.toDoId}`);
  }

  public save(options: { toDo: ToDo }): Observable<{ toDoId: number }> {
    return this._client.post<{ toDoId: number }>(`${this._baseUrl}api/toDos`, { toDo: options.toDo });
  }
}

