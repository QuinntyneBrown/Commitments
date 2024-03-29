// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { NotesService } from './notes.service';
import { Note } from './note';
import { Observable } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs';
import { ColDef } from 'ag-grid';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { DeleteCellComponent } from '../shared/delete-cell.component';
import { Store } from '../core/store';

@Component({
  templateUrl: './notes-page.component.html',
  styleUrls: ['./notes-page.component.scss'],
  selector: 'app-notes-page'
})
export class NotesPageComponent {
  constructor(
    private readonly _notesService: NotesService,
    private readonly _store: Store,
    private readonly _router: Router,
    private readonly _translateService: TranslateService
  ) {}

  public onDestroy: Subject<void> = new Subject<void>();

  public localeText: any = {};

  ngOnInit() {
    this._notesService
      .get()
      .pipe(map(x => this._store.notes$.next(x.notes)))
      .subscribe();

    this._translateService
      .get(['Title', 'Page', 'of', 'to'])
      .pipe(
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
    const notes = this._store.notes$.value;
    const deletedNoteIndex = notes.findIndex(x => x.noteId == $event.data.noteId);

    notes.splice(deletedNoteIndex, 1);

    this._notesService
      .remove({ note: <Note>$event.data })
      .pipe(
        takeUntil(this.onDestroy),
        tap(x => {
          this._store.notes$.next([...notes]);
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

  public get notes$(): Observable<Array<Note>> {
    return this._store.notes$;
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

