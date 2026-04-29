// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, TemplateRef, ViewChild, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { tap } from 'rxjs';
import { NotesService } from '../../../services/notes.service';
import { Note } from '../../../models/note';
import { Store } from '../../../core/store';
import { DataTableColumn, DataTableComponent, PrimaryHeaderComponent } from '@commitments/ui';

type NoteTableEvent = { data: Note };

@Component({
  selector: 'app-notes-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    RouterLink,
    MatButtonModule,
    MatIconModule,
    DataTableComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './notes-page.component.html',
  styleUrls: ['./notes-page.component.scss'],
})
export class NotesPageComponent {
  @ViewChild('titleTpl', { static: true })
  titleTpl!: TemplateRef<{ $implicit: Note }>;

  @ViewChild('deleteTpl', { static: true })
  deleteTpl!: TemplateRef<{ $implicit: Note }>;

  private readonly _notesService = inject(NotesService);
  private readonly _store = inject(Store);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly notes = this._store.notes;

  public columns: DataTableColumn<Note>[] = [];

  ngOnInit(): void {
    this.columns = [
      { key: 'title', header: 'Title', template: this.titleTpl },
      { key: 'delete', header: '', template: this.deleteTpl, width: '40px' },
    ];

    this._notesService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap((x) => this._store.notes.set(x.notes)),
      )
      .subscribe();
  }

  public handleDelete($event: NoteTableEvent): void {
    this._notesService
      .remove({ note: <Note>$event.data })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(() => {
          this._store.notes.update((notes) => notes.filter((x) => x.noteId != $event.data.noteId));
        }),
      )
      .subscribe();
  }
}
