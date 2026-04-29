// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { FrequencyService } from '../../../services/frequency.service';
import { Frequency } from '../../../models/frequency';
import { EditFrequencyDialogService } from '../../../services/edit-frequency-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type FrequencyRow = Frequency & { name?: string };
type FrequencyTableEvent = { data: Frequency };

@Component({
  selector: 'app-frequencies-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './frequencies-page.component.html',
  styleUrls: ['./frequencies-page.component.scss'],
})
export class FrequenciesPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: Frequency }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Frequency }>;

  private readonly _editFrequencyDialog = inject(EditFrequencyDialogService);
  private readonly _frequencyService = inject(FrequencyService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly frequencies = signal<Array<Frequency>>([]);
  public columns: DataTableColumn<Frequency>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (frequency) => (frequency as FrequencyRow).name ?? '' },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._frequencyService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.frequencies.set(x)),
      )
      .subscribe();
  }

  public handleRemoveClick($event: FrequencyTableEvent): void {
    this.frequencies.update((freqs) =>
      freqs.filter((x) => x.frequencyId != $event.data.frequencyId),
    );

    this._frequencyService
      .remove({ frequency: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick(_event: FrequencyTableEvent): void {}

  public handleFABButtonClick(): void {
    this._editFrequencyDialog
      .create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.addOrUpdate(x)),
      )
      .subscribe();
  }

  public addOrUpdate(frequency: Frequency): void {
    if (!frequency) return;

    this.frequencies.update((freqs) => {
      const next = [...freqs];
      const i = next.findIndex((t) => t.frequencyId == frequency.frequencyId);
      if (i < 0) {
        next.push(frequency);
      } else {
        next[i] = frequency;
      }
      return next;
    });
  }
}
