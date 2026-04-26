// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs';
import { Store } from '../core/store';
import { Tag } from '../models/tag';
import { TagsService } from './tags.service';

export const tagsResolver: ResolveFn<Tag[]> = (route, state) => {
  const tagsService = inject(TagsService);
  const store = inject(Store);

  return tagsService.get().pipe(tap(x => store.tags$.next(x.tags)), map(x => x.tags));
};
