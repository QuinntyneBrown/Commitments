// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { CardService } from '../../services/card.service';
import { Card } from '../../models/card';
import { tap } from 'rxjs';

@Component({
  selector: 'app-edit-card-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-card-dialog.html',
  styleUrls: ['./edit-card-dialog.scss']
})
export class EditCardDialogComponent {
  private readonly _cardService = inject(CardService);
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly card = signal<Card>(<Card>{});

  public cardId: number;

  ngOnInit() {
    if (this.cardId) {
      this._cardService.getById({ cardId: this.cardId })
        .pipe(
          takeUntilDestroyed(this._destroyRef),
          tap(x => {
            this.card.set(x);
            this.form.patchValue({
              name: x.name,
              description: x.description
            });
          })
        )
        .subscribe();
    }
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const card = new Card();
    card.cardId = this.cardId;
    card.description = this.form.value.description;
    card.name = this.form.value.name;
    this._cardService.save({ card })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => {
          card.cardId = x.cardId;
          this._overlay.close(card);
        })
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, []),
    description: new FormControl(null, [])
  });
}
