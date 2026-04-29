// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { BehaviourTypeService } from '../../../services/behaviour-type.service';
import { BehaviourType } from '../../../models/behaviour-type';
import { EditBehaviourTypeDialogService } from '../../../services/edit-behaviour-type-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type BehaviourTypeTableEvent = { data: BehaviourType };

@Component({
  selector: 'app-behaviour-types-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './behaviour-types-page.component.html',
  styleUrls: ['./behaviour-types-page.component.scss'],
})
export class BehaviourTypesPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: BehaviourType }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: BehaviourType }>;

  private readonly _behaviourTypeService = inject(BehaviourTypeService);
  private readonly _editBehaviourTypeDialog = inject(EditBehaviourTypeDialogService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly behaviourTypes = signal<Array<BehaviourType>>([]);
  public columns: DataTableColumn<BehaviourType>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'name', header: 'Name', cell: (behaviourType) => behaviourType.name },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._behaviourTypeService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.behaviourTypes.set(x)),
      )
      .subscribe();
  }

  public handleRemoveClick($event: BehaviourTypeTableEvent): void {
    this.behaviourTypes.update((types) =>
      types.filter((x) => x.behaviourTypeId != $event.data.behaviourTypeId),
    );

    this._behaviourTypeService
      .remove({ behaviourType: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick(_event: BehaviourTypeTableEvent): void {}

  public handleFABButtonClick(): void {
    this._editBehaviourTypeDialog
      .create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.addOrUpdate(x)),
      )
      .subscribe();
  }

  public addOrUpdate(behaviourType: BehaviourType): void {
    if (!behaviourType) return;

    this.behaviourTypes.update((types) => {
      const next = [...types];
      const i = next.findIndex((t) => t.behaviourTypeId == behaviourType.behaviourTypeId);
      if (i < 0) {
        next.push(behaviourType);
      } else {
        next[i] = behaviourType;
      }
      return next;
    });
  }
}
