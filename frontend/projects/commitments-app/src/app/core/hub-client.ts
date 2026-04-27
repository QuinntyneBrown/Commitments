// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, NgZone, inject, Inject } from '@angular/core';
import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@microsoft/signalr';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey, baseUrl, currentProfileIdKey } from './constants';

@Injectable({ providedIn: 'root' })
export class HubClient {
  private readonly _baseUrl = inject(baseUrl as any) as string;
  private readonly _storage = inject(LocalStorageService);
  private readonly _ngZone = inject(NgZone);

  private _connection: HubConnection | null = null;
  private _connect: Promise<void> | null = null;
  public messages$: Subject<any> = new Subject();

  public connect(): Promise<void> {
    if (this._connect) return this._connect;

    this._connect = new Promise<void>(resolve => {
      const options: IHttpConnectionOptions = {};

      const token = this._storage.get({ name: accessTokenKey });
      const profileId = this._storage.get({ name: currentProfileIdKey });
      this._connection = this._connection || new HubConnectionBuilder()
        .withUrl(`${this._baseUrl}hub?token=${token}&profileId=${profileId}`, options)
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
