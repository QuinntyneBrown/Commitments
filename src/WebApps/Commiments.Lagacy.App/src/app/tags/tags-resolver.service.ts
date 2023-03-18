// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Resolve } from '@angular/router';
import { Tag } from './tag';
import { Observable } from 'rxjs';
import { RouterStateSnapshot } from '@angular/router';
import { ActivatedRouteSnapshot } from '@angular/router';
import { TagsService } from './tags.service';
import { map, catchError, tap } from 'rxjs';
import { Store } from '../core/store';
import { Injectable } from '@angular/core';

@Injectable()
export class TagsResolver implements Resolve<Array<Tag>> {
  constructor(private readonly _tagsService: TagsService, private readonly _store: Store) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Tag[]> {
    return this._tagsService.get().pipe(tap(x => this._store.tags$.next(x.tags)), map(x => x.tags));
  }
}

