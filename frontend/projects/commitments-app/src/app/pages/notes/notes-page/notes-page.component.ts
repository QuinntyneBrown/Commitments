// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AgGridModule } from 'ag-grid-angular';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { ColDef } from 'ag-grid-community';
import { NotesService } from '../../../services/notes.service';
import { Note } from '../../../models/note';
import { Store } from '../../../core/store';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-notes-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './notes-page.component.html',
  styleUrls: ['./notes-page.component.scss']
})
export class NotesPageComponent {
  private readonly _notesService = inject(NotesService);
  private readonly _store = inject(Store);
  private readonly _router = inject(Router);
  private readonly _translateService = inject(TranslateService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly notes = this._store.notes;

  public localeText: any = {};

  ngOnInit() {
    this._notesService
      .get()
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => this._store.notes.set(x.notes))
      )
      .subscribe();

    this._translateService
      .get(['Title', 'Page', 'of', 'to'])
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(translations => {
          this.localeText = translations;
          this.columnDefs = [
            {
              headerName: translations['Title'],
              field: 'title',
              onCellClicked: $event => this.handleTitleClick($event)
            },
            {
              cellRenderer: 'deleteRenderer',
              onCellClicked: $event => this.handleDelete($event),
              width: 20
            }
          ];
        })
      )
      .subscribe();
  }

  public handleDelete($event) {
    this._notesService
      .remove({ note: <Note>$event.data })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(() => {
          this._store.notes.update(notes => notes.filter(x => x.noteId != $event.data.noteId));
        })
      )
      .subscribe();
  }

  public handleTitleClick($event) {
    this._router.navigateByUrl(`/notes/${$event.data.slug}`);
  }

  public frameworkComponents = {
    deleteRenderer: DeleteCellComponent
  };

  public columnDefs: Array<ColDef> = [];

  public onGridReady($event) {
    $event.api.sizeColumnsToFit();
  }
}
