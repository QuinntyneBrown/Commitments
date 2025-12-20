// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, Injector, Input } from '@angular/core';
import { Subject, pipe } from 'rxjs';
import { TagsService } from './tags.service';
import { Tag } from './tag';
import { Observable } from 'rxjs';
import { map, tap, filter } from 'rxjs';
import { ColDef } from 'ag-grid';
import { Overlay } from '@angular/cdk/overlay';
import { OverlayRefWrapper } from '../core/overlay-ref-wrapper';
import { ComponentPortal } from '@angular/cdk/portal';
import { AddTagDialog } from './add-tag-dialog/add-tag-dialog';
import { takeUntil } from 'rxjs';
import { MatSnackBar } from '@angular/material';
import { HubClient } from '../core/hub-client';
import { TranslateService } from '@ngx-translate/core';
import { DeleteCellComponent } from '../shared/delete-cell.component';
import { Store } from '../core/store';

@Component({
  templateUrl: './tags-page.component.html',
  styleUrls: ['./tags-page.component.scss'],
  selector: 'app-tags-page'
})
export class TagsPageComponent {
  constructor(
    private readonly _hubClient: HubClient,
    private readonly _injector: Injector,
    private readonly _overlay: Overlay,
    private readonly _snackBar: MatSnackBar,
    private readonly _store: Store,
    private readonly _tagsService: TagsService,
    private readonly _translateService: TranslateService
  ) {}

  public localeText: any = {};

  ngOnInit() {
    this._tagsService
      .get()
      .pipe(takeUntil(this.onDestroy), map(x => this._store.tags$.next(x.tags)))
      .subscribe();

    this._translateService
      .get(['Name', 'Page', 'of', 'to'])
      .pipe(
        tap(translations => {
          this.localeText = translations;
          this.columnDefs = [
            {
              headerName: translations['Name'],
              field: 'name',
              onCellValueChanged: $event => this.handleChange($event),
              editable: true
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

  public handleChange($event) {
    this._tagsService
      .save({ tag: $event.data })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public columnDefs: Array<ColDef> = [
    {
      headerName: 'Name',
      field: 'name',
      onCellValueChanged: $event => this.handleChange($event),
      editable: true
    },
    {
      cellRenderer: 'deleteRenderer',
      onCellClicked: $event => this.handleDelete($event),
      width: 20
    }
  ];

  public frameworkComponents = {
    deleteRenderer: DeleteCellComponent
  };

  public onGridReady(params) {
    params.api.sizeColumnsToFit();
  }

  public get tags$(): Observable<Array<Tag>> {
    return this._store.tags$;
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }

  public handleDelete($event) {
    this._tagsService
      .remove({ tagId: $event.data.tagId })
      .pipe(takeUntil(this.onDestroy))
      .subscribe();
  }

  public handleCreateClick() {
    const positionStrategy = this._overlay
      .position()
      .global()
      .centerHorizontally()
      .centerVertically();

    const overlayRef = this._overlay.create({
      hasBackdrop: true,
      positionStrategy
    });

    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);

    const injector = Injector.create({
      parent: this._injector,
      providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }],
    });
    const overlayPortal = new ComponentPortal(AddTagDialog, null, injector);
    overlayRef.attach(overlayPortal);

    overlayRef.backdropClick().subscribe(x => overlayRef.dispose());
  }
}

