// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { ActivityService } from '../../../services/activity.service';
import { Activity } from '../../../models/activity';
import { EditActivityDialogService } from '../../../services/edit-activity-dialog.service';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type ActivityRow = Activity & { behaviour?: { name?: string } };
type ActivityTableEvent = { data: Activity };

@Component({
  selector: 'app-activities-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './activities-page.component.html',
  styleUrls: ['./activities-page.component.scss'],
})
export class ActivitiesPageComponent {
  @ViewChild('editTpl', { static: true })
  editTpl!: TemplateRef<{ $implicit: Activity }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Activity }>;

  private readonly _activityService = inject(ActivityService);
  private readonly _editActivityDialog = inject(EditActivityDialogService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly activities = signal<Array<Activity>>([]);
  public columns: DataTableColumn<Activity>[] = [];

  ngOnInit(): void {
    this.columns = [
      {
        key: 'behaviour',
        header: 'Behaviour',
        cell: (activity) => (activity as ActivityRow).behaviour?.name ?? '',
      },
      {
        key: 'performedOn',
        header: 'Performed On',
        cell: (activity) => activity.performedOn ?? '',
      },
      { key: 'edit', header: '', template: this.editTpl, width: '50px' },
      { key: 'delete', header: '', template: this.deleteTpl, width: '50px' },
    ];

    this._activityService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.activities.set(x)),
      )
      .subscribe();
  }

  public handleRemoveClick($event: ActivityTableEvent): void {
    this.activities.update((activities) =>
      activities.filter((x) => x.activityId != $event.data.activityId),
    );

    this._activityService
      .remove({ activity: $event.data })
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe();
  }

  public handleEditClick($event: ActivityTableEvent): void {
    this._editActivityDialog
      .create({ activityId: $event.data.activityId })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.addOrUpdate(x)),
      )
      .subscribe();
  }

  public handleFABButtonClick(): void {
    this._editActivityDialog
      .create()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this.addOrUpdate(x)),
      )
      .subscribe();
  }

  public addOrUpdate(activity: Activity): void {
    if (!activity) return;

    this.activities.update((activities) => {
      const next = [...activities];
      const i = next.findIndex((t) => t.activityId == activity.activityId);
      if (i < 0) {
        next.push(activity);
      } else {
        next[i] = activity;
      }
      return next;
    });
  }
}
