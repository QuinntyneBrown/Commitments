// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, NgZone, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from "@microsoft/signalr";
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey, baseUrl } from './constants';

@Injectable()
export class HubClient {
  private _connection: HubConnection | null = null;
  private _connect: Promise<void> | null = null;
  public messages$: Subject<any> = new Subject();

  constructor(
    @Inject(baseUrl) private readonly _baseUrl: string,
    private readonly _storage: LocalStorageService,
    private readonly _ngZone: NgZone
  ) {}

  public connect(): Promise<void> {
    if (this._connect) return this._connect;

    this._connect = new Promise<void>(resolve => {

      const options: IHttpConnectionOptions = { };

      this._connection = this._connection || new HubConnectionBuilder()
        .withUrl(`${this._baseUrl}hub?token=${this._storage.get({ name: accessTokenKey })}`, options)
        .build();

      this._connection.on('message', value => {
        this._ngZone.run(() => this.messages$.next(value));
      });

      this._connection.start().then(() => resolve());
    });

    return this._connect;
  }

  public disconnect() {
    if (this._connection) {
      this._connection.stop();
      this._connect = null;
      this._connection = null;
    }
  }
}

