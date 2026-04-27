// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, NgZone, inject } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { HubConnection, HubConnectionBuilder, IHttpConnectionOptions } from '@microsoft/signalr';
import { LocalStorageService } from './local-storage.service';
import { accessTokenKey, baseUrl, currentProfileIdKey } from './constants';
import { HubRetryPolicy } from './hub-retry-policy';
import { MessageIdempotenceCache } from './message-idempotence-cache';

export interface RealtimeMessage<TPayload> {
  schemaVersion: 1;
  messageId: string;
  event: string;
  profileId: string;
  occurredAt: string;
  correlationId: string | null;
  payload: TPayload;
}

function isEnvelope(value: unknown, event: string): boolean {
  const m = value as Partial<RealtimeMessage<unknown>> | null;
  return !!m && m.schemaVersion === 1 && m.event === event;
}

@Injectable({ providedIn: 'root' })
export class HubClient {
  private readonly _baseUrl = inject(baseUrl as any) as string;
  private readonly _storage = inject(LocalStorageService);
  private readonly _ngZone = inject(NgZone);

  private _connection: HubConnection | null = null;
  private _connect: Promise<void> | null = null;
  private readonly _idempotence = new MessageIdempotenceCache();
  public messages$: Subject<any> = new Subject();

  public connect(): Promise<void> {
    if (this._connect) return this._connect;

    this._connect = new Promise<void>(resolve => {
      const options: IHttpConnectionOptions = {};

      const token = this._storage.get({ name: accessTokenKey });
      const profileId = this._storage.get({ name: currentProfileIdKey });
      this._connection = this._connection || new HubConnectionBuilder()
        .withUrl(`${this._baseUrl}hub?token=${token}&profileId=${profileId}`, options)
        .withAutomaticReconnect(HubRetryPolicy)
        .build();

      this._connection.on('message', value => {
        if (this._idempotence.seen(value?.messageId)) return;
        this._ngZone.run(() => this.messages$.next(value));
      });

      this._connection.onreconnected(() => {
        this._ngZone.run(() => this.messages$.next({
          schemaVersion: 1,
          messageId: this._randomId(),
          event: 'hubResumed',
          profileId,
          occurredAt: new Date().toISOString(),
          correlationId: null,
          payload: {}
        } as RealtimeMessage<{}>));
      });

      this._connection.start().then(() => resolve());
    });

    return this._connect;
  }

  private _randomId(): string {
    const c: any = (globalThis as any).crypto;
    if (c?.randomUUID) return c.randomUUID();
    return `${Date.now()}-${Math.random().toString(36).slice(2)}`;
  }

  public disconnect() {
    if (this._connection) {
      this._connection.stop();
      this._connect = null;
      this._connection = null;
    }
  }

  public on<TPayload>(event: string): Observable<TPayload> {
    return this.messages$.pipe(
      filter((m): m is RealtimeMessage<TPayload> => isEnvelope(m, event)),
      map(m => m.payload)
    );
  }
}
