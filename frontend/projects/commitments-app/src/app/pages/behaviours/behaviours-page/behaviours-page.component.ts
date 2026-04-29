// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { BehaviourService } from '../../../services/behaviour.service';
import { Behaviour } from '../../../models/behaviour';
import { EditBehaviourDialogService } from '../../../services/edit-behaviour-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type BehaviourTableEvent = { data: Behaviour };

@Component({
  selector: 'app-edit-behaviour-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './behaviours-page.component.html',
  styleUrls: ['./behaviours-page.component.scss'],
})
export class BehavioursPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: Behaviour }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Behaviour }>;

  private readonly _behaviourService = inject(BehaviourService);
  private readonly _editBehaviourDialog = inject(EditBehaviourDialogService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly behaviour = signal<Behaviour>(<Behaviour>{});
  public readonly behaviours = signal<Array<Behaviour>>([]);
  public columns: DataTableColumn<Behaviour>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (behaviour) => behaviour.name },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._behaviourService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.behaviours.set(x)),
      )
      .subscribe();
  }

  public handleFABButtonClick(): void {
    this._editBehaviourDialog
      .create({ behaviourId: this.behaviour().behaviourId })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((b) => this.addOrUpdate(b)),
      )
      .subscribe();
  }

  public handleRemoveClick($event: BehaviourTableEvent): void {
    this.behaviours.update((bs) => bs.filter((x) => x.behaviourId != $event.data.behaviourId));

    this._behaviourService
      .remove({ behaviour: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick($event: BehaviourTableEvent): void {
    this._editBehaviourDialog
      .create({ behaviourId: $event.data.behaviourId })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((b) => this.addOrUpdate(b)),
      )
      .subscribe();
  }

  public addOrUpdate(behaviour: Behaviour): void {
    if (!behaviour) return;

    this.behaviours.update((bs) => {
      const next = [...bs];
      const i = next.findIndex((t) => t.behaviourId == behaviour.behaviourId);
      if (i < 0) {
        next.push(behaviour);
      } else {
        next[i] = behaviour;
      }
      return next;
    });
  }
}
