// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { CommitmentService } from '../../../services/commitment.service';
import { Commitment } from '../../../models/commitment';
import { EditCommitmentDialogService } from '../../../services/edit-commitment-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type CommitmentRow = Commitment & {
  behaviour?: {
    name?: string;
    behaviourType?: { name?: string };
  };
};
type CommitmentTableEvent = { data: Commitment };

@Component({
  selector: 'app-commitments-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './commitments-page.component.html',
  styleUrls: ['./commitments-page.component.scss'],
})
export class CommitmentsPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: Commitment }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Commitment }>;

  private readonly _commitmentService = inject(CommitmentService);
  private readonly _editCommitmentDialog = inject(EditCommitmentDialogService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly commitments = signal<Commitment[]>([]);
  public columns: DataTableColumn<Commitment>[] = [];

  ngOnInit(): void {
    this.columns = [
      {
        key: 'type',
        header: 'Type',
        cell: (commitment) => (commitment as CommitmentRow).behaviour?.behaviourType?.name ?? '',
      },
      {
        key: 'name',
        header: 'Name',
        cell: (commitment) => (commitment as CommitmentRow).behaviour?.name ?? '',
      },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._commitmentService
      .getPersonal()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.commitments.set(x)),
      )
      .subscribe();
  }

  public handleFABButtonClick(): void {
    this._editCommitmentDialog.create().pipe(takeUntilDestroyed(this._destroyRef)).subscribe();
  }

  public handleEditClick($event: CommitmentTableEvent): void {
    this._editCommitmentDialog
      .create({ commitmentId: $event.data.commitmentId })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((commitment) => this.addOrUpdate(commitment)),
      )
      .subscribe();
  }

  public handleRemoveClick($event: CommitmentTableEvent): void {
    const commitment = $event.data;

    this.commitments.update((commitments) =>
      commitments.filter((x) => x.commitmentId != commitment.commitmentId),
    );

    this._commitmentService
      .remove({ commitment })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public addOrUpdate(commitment: Commitment): void {
    if (!commitment) return;

    this.commitments.update((commitments) => {
      const next = [...commitments];
      const i = next.findIndex((t) => t.commitmentId == commitment.commitmentId);
      if (i < 0) {
        next.push(commitment);
      } else {
        next[i] = commitment;
      }
      return next;
    });
  }
}
