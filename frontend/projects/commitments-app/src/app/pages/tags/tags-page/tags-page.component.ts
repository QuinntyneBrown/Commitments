// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, Injector } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgGridModule } from 'ag-grid-angular';
import { Overlay } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { tap } from 'rxjs';
import { ColDef } from 'ag-grid-community';
import { TagsService } from '../../../services/tags.service';
import { Store } from '../../../core/store';
import { HubClient } from '../../../core/hub-client';
import { OverlayRefWrapper } from '../../../core/overlay-ref-wrapper';
import { AddTagDialogComponent } from '../../../components/add-tag-dialog/add-tag-dialog.component';
import { DeleteCellComponent } from '../../../components/delete-cell/delete-cell.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-tags-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    AgGridModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './tags-page.component.html',
  styleUrls: ['./tags-page.component.scss']
})
export class TagsPageComponent {
  private readonly _hubClient = inject(HubClient);
  private readonly _injector = inject(Injector);
  private readonly _overlay = inject(Overlay);
  private readonly _snackBar = inject(MatSnackBar);
  private readonly _store = inject(Store);
  private readonly _tagsService = inject(TagsService);
  private readonly _translateService = inject(TranslateService);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly tags = this._store.tags;

  public localeText: any = {};

  ngOnInit() {
    this._tagsService
      .get()
      .pipe(takeUntilDestroyed(this._destroyRef), tap(x => this._store.tags.set(x.tags)))
      .subscribe();

    this._translateService
      .get(['Name', 'Page', 'of', 'to'])
      .pipe(
        takeUntilDestroyed(this._destroyRef),
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
      .pipe(takeUntilDestroyed(this._destroyRef))
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

  public handleDelete($event) {
    this._tagsService
      .remove({ tagId: $event.data.tagId })
      .pipe(takeUntilDestroyed(this._destroyRef))
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
    const overlayPortal = new ComponentPortal(AddTagDialogComponent, null, injector);
    overlayRef.attach(overlayPortal);

    overlayRef.backdropClick().subscribe(x => overlayRef.dispose());
  }
}
