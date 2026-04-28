// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { CardLayoutService } from '../../services/card-layout.service';
import { CardLayout } from '../../models/card-layout';
import { tap } from 'rxjs';

@Component({
  selector: 'app-edit-card-layout-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-card-layout-dialog.html',
  styleUrls: ['./edit-card-layout-dialog.scss']
})
export class EditCardLayoutDialogComponent {
  private readonly _cardLayoutService = inject(CardLayoutService);
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly cardLayout = signal<CardLayout>(<CardLayout>{});

  public cardLayoutId: number;

  ngOnInit() {
    if (this.cardLayoutId) {
      this._cardLayoutService.getById({ cardLayoutId: this.cardLayoutId })
        .pipe(
          takeUntilDestroyed(this._destroyRef),
          tap(x => {
            this.cardLayout.set(x);
            this.form.patchValue({ name: x.name });
          })
        )
        .subscribe();
    }
  }

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const cardLayout = new CardLayout();
    cardLayout.cardLayoutId = this.cardLayoutId;
    cardLayout.name = this.form.value.name;
    this._cardLayoutService.save({ cardLayout })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => {
          cardLayout.cardLayoutId = x.cardLayoutId;
          this._overlay.close(cardLayout);
        })
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [])
  });
}
