// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { CardService } from '../../../services/card.service';
import { Card } from '../../../models/card';
import { EditCardDialogService } from '../../../services/edit-card-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type CardTableEvent = { data: Card };

@Component({
  selector: 'app-cards-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './cards-page.component.html',
  styleUrls: ['./cards-page.component.scss'],
})
export class CardsPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: Card }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Card }>;

  private readonly _cardService = inject(CardService);
  private readonly _editCardDialog = inject(EditCardDialogService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly cards = signal<Card[]>([]);
  public columns: DataTableColumn<Card>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (card) => card.name },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._cardService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((cards) => this.cards.set(cards)),
      )
      .subscribe();
  }

  public handleFABButtonClick(): void {
    this._editCardDialog
      .create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.addOrUpdate(x)),
      )
      .subscribe();
  }

  public handleEditClick($event: CardTableEvent): void {
    this._editCardDialog
      .create({ cardId: $event.data.cardId })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((card) => this.addOrUpdate(card)),
      )
      .subscribe();
  }

  public handleRemove($event: CardTableEvent): void {
    const card = $event.data;

    this.cards.update((cards) => cards.filter((x) => x.cardId != card.cardId));

    this._cardService.remove({ card }).pipe(takeUntilDestroyed(this._destroyRef)).subscribe();
  }

  public addOrUpdate(card: Card): void {
    if (!card) return;

    this.cards.update((cards) => {
      const next = [...cards];
      const i = next.findIndex((t) => t.cardId == card.cardId);
      if (i < 0) {
        next.push(card);
      } else {
        next[i] = card;
      }
      return next;
    });
  }
}
