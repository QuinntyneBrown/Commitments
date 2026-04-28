// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { EditFrequencyDialogService } from '../../../services/edit-frequency-dialog.service';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-edit-frequency-page',
  standalone: true,
  imports: [CommonModule, TranslateModule, PrimaryHeaderComponent],
  templateUrl: './edit-frequency-page.component.html',
  styleUrls: ['./edit-frequency-page.component.scss']
})
export class EditFrequencyPageComponent {
  private readonly _dialog = inject(EditFrequencyDialogService);
  private readonly _route = inject(ActivatedRoute);
  private readonly _router = inject(Router);

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnInit() {
    const frequencyIdParam = this._route.snapshot.paramMap.get('frequencyId');
    const returnTo = this._route.snapshot.queryParamMap.get('returnTo') ?? '/frequencies';
    const frequencyId = frequencyIdParam ? Number(frequencyIdParam) : undefined;

    this._dialog.create({ frequencyId })
      .pipe(takeUntil(this.onDestroy))
      .subscribe(() => this._router.navigateByUrl(returnTo));
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
