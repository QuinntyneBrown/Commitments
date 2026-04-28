// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { OverlayRefWrapper } from '../../core/overlay-ref-wrapper';
import { BehaviourService } from '../../services/behaviour.service';
import { Behaviour } from '../../models/behaviour';
import { tap } from 'rxjs';
import { BehaviourType } from '../../models/behaviour-type';
import { BehaviourTypeService } from '../../services/behaviour-type.service';

@Component({
  selector: 'app-edit-behaviour-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-behaviour-dialog.html',
  styleUrls: ['./edit-behaviour-dialog.scss']
})
export class EditBehaviourDialogComponent {
  private readonly _behaviourService = inject(BehaviourService);
  private readonly _behaviourTypeService = inject(BehaviourTypeService);
  private readonly _overlay = inject(OverlayRefWrapper);
  private readonly _destroyRef = inject(DestroyRef);

  public readonly behaviourTypes = toSignal(this._behaviourTypeService.get(), { initialValue: [] as BehaviourType[] });
  public readonly behaviour = signal<Behaviour>(<Behaviour>{});

  public behaviourId: number;

  ngOnInit() {
    if (this.behaviourId) {
      this._behaviourService.getById({ behaviourId: this.behaviourId })
        .pipe(
          takeUntilDestroyed(this._destroyRef),
          tap(x => {
            this.behaviour.set(x);
            this.form.patchValue({
              name: x.name,
              description: x.description,
              behaviourTypeId: x.behaviourTypeId,
              isDesired: x.isDesired
            });
          })
        )
        .subscribe();
    }
  }

  public handleCancelClick() { this._overlay.close(); }

  public handleSaveClick() {
    const behaviour = new Behaviour();
    behaviour.isDesired = this.form.value.isDesired;
    behaviour.behaviourTypeId = this.form.value.behaviourTypeId;
    behaviour.name = this.form.value.name;
    behaviour.description = this.form.value.description;

    this._behaviourService.save({ behaviour })
      .pipe(
        takeUntilDestroyed(this._destroyRef),
        tap(x => {
          behaviour.behaviourId = x.behaviourId;
          this._overlay.close(behaviour);
        })
      )
      .subscribe();
  }

  public form = new FormGroup({
    name: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    isDesired: new FormControl(true, [Validators.required]),
    behaviourTypeId: new FormControl(null, [Validators.required])
  });
}
