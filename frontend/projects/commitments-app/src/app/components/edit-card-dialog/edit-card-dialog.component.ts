// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Subject, BehaviorSubject } from 'rxjs';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { CardService } from '../../services/card.service';
import { Card } from '../../models/card';
import { map, switchMap, tap, takeUntil } from 'rxjs';

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

  ngOnInit() {
    if (this.cardId)
      this._cardService.getById({ cardId: this.cardId })
        .pipe(
          map(x => this.card$.next(x)),
          switchMap(x => this.card$),
          map(x => this.form.patchValue({
            name: x.name,
            description: x.description
          }))
        )
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public card$: BehaviorSubject<Card> = new BehaviorSubject(<Card>{});

  public cardId: number;

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
        map(x => card.cardId = x.cardId),
        tap(x => this._overlay.close(card)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, []),
    description: new FormControl(null, [])
  });
}
