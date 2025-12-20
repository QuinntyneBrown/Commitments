// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { OverlayRefWrapper } from '../core/overlay-ref-wrapper';
import { TagsService } from './tags.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Tag } from './tag';
import { map, tap, takeUntil } from 'rxjs';
import { Store } from '../core/store';

@Component({
  templateUrl: './add-tag-dialog.html',
  styleUrls: ['./add-tag-dialog.scss'],
  selector: 'app-add-tag-dialog'
})
export class AddTagDialog {
  constructor(
    private readonly _overlay: OverlayRefWrapper,
    private readonly _store: Store,
    private readonly _tagService: TagsService
  ) {}

  public handleCancel() {
    this._overlay.close();
  }

  public handleSave(tag: Tag) {
    this._tagService
      .save({ tag })
      .pipe(
        takeUntil(this.onDestroy),
        map((result: any) => {
          tag.tagId = result.tagId;
          this._store.tags$.next([...this._store.tags$.value, tag]);
        }),
        tap(() => this._overlay.close())
      )
      .subscribe();
  }

  public tag: Tag = <Tag>{};

  public form = new FormGroup({
    name: new FormControl(this.tag.name, [Validators.required])
  });

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

