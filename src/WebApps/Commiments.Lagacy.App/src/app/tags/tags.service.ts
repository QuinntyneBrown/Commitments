// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { baseUrl } from '../core/constants';
import { Tag } from './tag';
import { Observable } from 'rxjs';
import { shareReplay, tap } from 'rxjs';
import { HubClient } from '../core/hub-client';

@Injectable()
export class TagsService {
  constructor(
    private readonly _httpClient: HttpClient,
    private readonly _hubClient: HubClient,
    @Inject(baseUrl) private readonly _baseUrl: string
  ) {
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

  private readonly _get(): Observable<{ tags: Array<Tag> }> {
    return this._httpClient.get<{ tags: Array<Tag> }>(`${this._baseUrl}api/tags`);
  }

  private readonly _cache$: Observable<{ tags: Array<Tag> }>;
}

