// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { TagsService } from '../../services/tags.service';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Tag } from '../../models/tag';
import { map, tap, takeUntil } from 'rxjs';
import { Store } from '../../core/store';

@Component({
  selector: 'app-add-tag-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-tag-dialog.html',
  styleUrls: ['./add-tag-dialog.scss']
})
export class AddTagDialogComponent {
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _store = inject(Store);
  private readonly _tagService = inject(TagsService);

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
