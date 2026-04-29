// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, computed, signal } from '@angular/core';
import { DashboardMode } from '@commitments/dashboard-framework';

import { RelationsSummaryDto, RelationsSummaryService } from '../../data/relations-summary.service';

@Injectable()
export class RelationsController {
  readonly snapshot = signal<RelationsSummaryDto | null>(null);
  readonly mode = signal<DashboardMode>('live');

  readonly relations = computed(() => this.snapshot()?.relations ?? []);
  readonly isEmpty = computed(() => this.relations().length === 0);

  constructor(private readonly _service: RelationsSummaryService) {}

  async load(mode: DashboardMode, asOf: string | null): Promise<void> {
    this.mode.set(mode);
    this.snapshot.set(await this._service.get(asOf));
  }
}
