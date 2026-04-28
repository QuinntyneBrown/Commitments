// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { TagsService } from '../../services/tags.service';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Tag } from '../../models/tag';
import { map, tap } from 'rxjs';
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
  private readonly _destroyRef = inject(DestroyRef);

  public handleCancel() {
    this._overlay.close();
  }

  public handleSave(tag: Tag) {
    this._tagService
      .save({ tag })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        map((result: any) => {
          tag.tagId = result.tagId;
          this._store.tags.update(tags => [...tags, tag]);
        }),
        tap(() => this._overlay.close())
      )
      .subscribe();
  }

  public tag: Tag = <Tag>{};

  public form = new FormGroup({
    name: new FormControl(this.tag.name, [Validators.required])
  });
}
