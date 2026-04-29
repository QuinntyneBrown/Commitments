// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, Injector, TemplateRef, ViewChild, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Overlay } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { tap } from 'rxjs';
import { TagsService } from '../../../services/tags.service';
import { Store } from '../../../core/store';
import { OverlayRefWrapper } from '../../../core/overlay-ref-wrapper';
import { AddTagDialogComponent } from '../../../components/add-tag-dialog/add-tag-dialog.component';
import { Tag } from '../../../models/tag';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type TagTableEvent = { data: Tag };

@Component({
  selector: 'app-tags-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './tags-page.component.html',
  styleUrls: ['./tags-page.component.scss'],
})
export class TagsPageComponent {
  @ViewChild('nameTpl', { static: true })
  nameTpl!: TemplateRef<{ $implicit: Tag }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Tag }>;

  private readonly _injector = inject(Injector);
  private readonly _overlay = inject(Overlay);
  private readonly _store = inject(Store);
  private readonly _tagsService = inject(TagsService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly tags = this._store.tags;
  public columns: DataTableColumn<Tag>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', template: this.nameTpl },
      { key: 'delete', header: '', template: this.deleteTpl, width: '40px' },
    ];

    this._tagsService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this._store.tags.set(x.tags)),
      )
      .subscribe();
  }

  public onNameInput(row: Tag, value: string): void {
    row.name = value;
  }

  public handleChange($event: TagTableEvent): void {
    this._tagsService
      .save({ tag: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleDelete($event: TagTableEvent): void {
    this._tagsService
      .remove({ tagId: $event.data.tagId })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleCreateClick(): void {
    const positionStrategy = this._overlay
      .position()
      .global()
      .centerHorizontally()
      .centerVertically();

    const overlayRef = this._overlay.create({
      hasBackdrop: true,
      positionStrategy,
    });

    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);

    const injector = Injector.create({
      parent: this._injector,
      providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }],
    });
    const overlayPortal = new ComponentPortal(AddTagDialogComponent, null, injector);
    overlayRef.attach(overlayPortal);

    overlayRef.backdropClick().subscribe(() => overlayRef.dispose());
  }
}
