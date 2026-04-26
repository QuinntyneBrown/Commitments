// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject, Injector } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Overlay } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs';
import { FrequencyService } from '../../services/frequency.service';
import { FrequencyTypeService } from '../../services/frequency-type.service';
import { FrequencyType } from '../../models/frequency-type';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { EditFrequencyOverlayComponent } from '../../components/frequencies-editor/edit-frequency-overlay.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-edit-frequency-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatButtonModule,
    MatIconModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './edit-frequency-page.component.html',
  styleUrls: ['./edit-frequency-page.component.scss']
})
export class EditFrequencyPageComponent {
  private readonly _frequencyService = inject(FrequencyService);
  private readonly _frequencyTypeService = inject(FrequencyTypeService);
  private readonly _overlay = inject(Overlay);
  private readonly _injector = inject(Injector);

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
