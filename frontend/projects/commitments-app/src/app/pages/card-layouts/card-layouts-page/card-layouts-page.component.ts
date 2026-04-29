// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { CardLayoutService } from '../../../services/card-layout.service';
import { CardLayout } from '../../../models/card-layout';
import { EditCardLayoutDialogService } from '../../../services/edit-card-layout-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type CardLayoutTableEvent = { data: CardLayout };

@Component({
  selector: 'app-card-layouts-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './card-layouts-page.component.html',
  styleUrls: ['./card-layouts-page.component.scss'],
})
export class CardLayoutsPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: CardLayout }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: CardLayout }>;

  private readonly _cardLayoutService = inject(CardLayoutService);
  private readonly _editCardLayoutDialog = inject(EditCardLayoutDialogService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly cardLayouts = signal<Array<CardLayout>>([]);
  public columns: DataTableColumn<CardLayout>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (cardLayout) => cardLayout.name },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._cardLayoutService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.cardLayouts.set(x)),
      )
      .subscribe();
  }

  public handleRemoveClick($event: CardLayoutTableEvent): void {
    this.cardLayouts.update((layouts) =>
      layouts.filter((x) => x.cardLayoutId != $event.data.cardLayoutId),
    );

    this._cardLayoutService
      .remove({ cardLayout: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick(_event: CardLayoutTableEvent): void {}

  public addOrUpdate(cardLayout: CardLayout): void {
    if (!cardLayout) return;

    this.cardLayouts.update((layouts) => {
      const next = [...layouts];
      const i = next.findIndex((t) => t.cardLayoutId == cardLayout.cardLayoutId);
      if (i < 0) {
        next.push(cardLayout);
      } else {
        next[i] = cardLayout;
      }
      return next;
    });
  }

  public handleFABButtonClick(): void {
    this._editCardLayoutDialog
      .create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.addOrUpdate(x)),
      )
      .subscribe();
  }
}
