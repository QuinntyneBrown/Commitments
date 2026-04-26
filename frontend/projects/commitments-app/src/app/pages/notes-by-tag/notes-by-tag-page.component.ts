// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs';
import { TagsService } from '../../services/tags.service';
import { Tag } from '../../models/tag';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-notes-by-tag-page',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    MatCardModule,
    PrimaryHeaderComponent,
  ],
  templateUrl: './notes-by-tag-page.component.html',
  styleUrls: ['./notes-by-tag-page.component.scss']
})
export class NotesByTagPageComponent {
  private readonly _activatedRoute = inject(ActivatedRoute);
  private readonly _tagsService = inject(TagsService);

  ngOnInit() {
    this.tag$ = this._tagsService.getBySlug({ slug: this.slug }).pipe(map(x => x.tag));
  }

  public tag$: Observable<Tag>;

  public onDestroy: Subject<void> = new Subject<void>();

  public get slug() {
    return this._activatedRoute.snapshot.params['slug'];
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
