// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { TagsService } from '../tags/tags.service';
import { Observable } from 'rxjs';
import { Tag } from '../tags/tag';
import { map } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  templateUrl: './notes-by-tag-page.component.html',
  styleUrls: ['./notes-by-tag-page.component.scss'],
  selector: 'app-notes-by-tag-page'
})
export class NotesByTagPageComponent {
  constructor(private readonly _activatedRoute: ActivatedRoute, private readonly _tagsService: TagsService) {}

  ngOnInit() {
    this.tag$ = this._tagsService.getBySlug({ slug: this.slug }).pipe(map(x => x.tag));
  }

  public tag$: Observable<Tag>;

  public onDestroy: Subject<void> = new Subject<void>();

  public get slug() {
    return this._activatedRoute.snapshot.params['slug'];
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

