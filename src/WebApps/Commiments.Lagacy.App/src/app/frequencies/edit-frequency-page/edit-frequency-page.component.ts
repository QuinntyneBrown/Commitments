// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, Injector } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { Overlay } from '@angular/cdk/overlay';
import { FrequencyService } from './frequency.service';
import { OverlayRefWrapper } from '../core/overlay-ref-wrapper';
import { ComponentPortal } from '@angular/cdk/portal';
import { EditFrequencyOverlayComponent } from './edit-frequency-overlay.component';
import { takeUntil, map } from 'rxjs';
import { FrequencyTypeService } from './frequency-type.service';
import { FrequencyType } from './frequency-type';

@Component({
  templateUrl: './edit-frequency-page.component.html',
  styleUrls: ['./edit-frequency-page.component.scss'],
  selector: 'app-edit-frequency-page',
})
export class EditFrequencyPageComponent {
  constructor(
    private readonly _frequencyService: FrequencyService,
    private readonly _frequencyTypeService: FrequencyTypeService,
    private readonly _overlay: Overlay,
    private readonly _injector: Injector
  ) {}

  frequencyTypes: Array<FrequencyType>;

  ngOnInit() {
    this._frequencyTypeService
      .get()
      .pipe(
        map((x) => (this.frequencyTypes = x)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public handleFabButtonClick() {
    const positionStrategy = this._overlay
      .position()
      .global()
      .centerHorizontally()
      .centerVertically();

    const overlayRef = this._overlay.create({
      hasBackdrop: true,
      positionStrategy,
    });

    const overlayRefWrapper = new OverlayRefWrapper(overlayRef);
    overlayRefWrapper.data = {
      frequencyTypes: this.frequencyTypes,
    };

    const injector = Injector.create({
      parent: this._injector,
      providers: [{ provide: OverlayRefWrapper, useValue: overlayRefWrapper }],
    });
    const overlayPortal = new ComponentPortal(EditFrequencyOverlayComponent, null, injector);

    overlayRef.attach(overlayPortal);

    overlayRefWrapper
      .afterClosed()
      .pipe(
        takeUntil(this.onDestroy),
        map((x) => console.log(x))
      )
      .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();
  }
}

