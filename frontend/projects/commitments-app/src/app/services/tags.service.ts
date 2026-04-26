// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { baseUrl } from '../core/constants';
import { Tag } from '../models/tag';
import { Observable } from 'rxjs';
import { shareReplay, tap } from 'rxjs';
import { HubClient } from '../core/hub-client';

@Injectable({ providedIn: 'root' })
export class TagsService {
  private readonly _httpClient = inject(HttpClient);
  private readonly _hubClient = inject(HubClient);
  private readonly _baseUrl = inject(baseUrl as any) as string;

  private _cache$: Observable<{ tags: Array<Tag> }>;

  constructor() {
    this._hubClient.messages$.pipe(tap(() => (this._cache$ = null))).subscribe();
  }

  public save(options) {
    return this._httpClient.post<{ tag: Tag }>(`${this._baseUrl}api/tags`, options);
  }

  public get(): Observable<{ tags: Array<Tag> }> {
    if (!this._cache$) {
      this._cache$ = this._get().pipe(shareReplay(1));
    }

    return this._cache$;
  }

  public getBySlug(options: { slug: string }): Observable<{ tag: Tag }> {
    return this._httpClient.get<{ tag: Tag }>(`${this._baseUrl}api/tags/slug/${options.slug}`);
  }

  public remove(options) {
    return this._httpClient.delete<{ tags: Array<Tag> }>(
      `${this._baseUrl}api/tags/${options.tagId}`
    );
  }

  private _get(): Observable<{ tags: Array<Tag> }> {
    return this._httpClient.get<{ tags: Array<Tag> }>(`${this._baseUrl}api/tags`);
  }
}
