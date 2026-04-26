// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Component, ElementRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { takeUntil, tap } from 'rxjs';
import { NotesService } from '../../services/notes.service';
import { Note } from '../../models/note';
import { Tag } from '../../models/tag';
import { Store } from '../../core/store';
import { LanguageService } from '../../core/language.service';
import { LocalStorageService } from '../../core/local-storage.service';
import { QuillTextEditorComponent } from '../../components/quill-text-editor/quill-text-editor.component';
import { AutoCompleteChipListComponent } from '../../components/auto-complete-chip-list/auto-complete-chip-list.component';
import { PrimaryHeaderComponent } from '@commitments/ui';

@Component({
  selector: 'app-edit-note-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    TranslateModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    QuillTextEditorComponent,
    AutoCompleteChipListComponent,
    PrimaryHeaderComponent,
  ],
  templateUrl: './edit-note-page.component.html',
  styleUrls: ['./edit-note-page.component.scss']
})
export class EditNotePageComponent {
  private readonly _activatedRoute = inject(ActivatedRoute);
  private readonly _elementRef = inject(ElementRef);
  private readonly _languageService = inject(LanguageService);
  private readonly _localStorageService = inject(LocalStorageService);
  private readonly _notesService = inject(NotesService);
  private readonly _router = inject(Router);
  private readonly _store = inject(Store);

  selectedItems: any[];
  items: any[];

  public editorPlaceholder: string = 'Compose a note...';

  public notes$: BehaviorSubject<Note> = new BehaviorSubject(<Note>{});

  public onDestroy: Subject<void> = new Subject();

  public quillEditorFormControl: FormControl = new FormControl('');

  public form = new FormGroup({
    title: new FormControl(null, [Validators.required]),
    body: new FormControl(null, [Validators.required]),
    tags: new FormControl()
  });

  constructor() {
    this.editorPlaceholder = this._languageService.currentTranslations[this.editorPlaceholder];

    this.selectedItems = this._store.note$.value.tags.map(x => x.name);
    this.items = this._store.tags$.value;

    this.form.controls['title'].setValue(this._store.note$.value.title);
    this.form.controls['body'].setValue(this._store.note$.value.body);
  }

  canDeactivate() {
    !this.form.dirty;
  }

  ngAfterViewInit() {
    this.form.patchValue({
      title: this.note$.value.title,
      body: this.note$.value.body
    });
  }

  public get tags$(): Observable<Array<Tag>> {
    return this._store.tags$;
  }

  public get note$(): BehaviorSubject<Note> {
    return this._store.note$;
  }

  public handleSaveClick() {
    let note = new Note();
    let tags = this.form.value.tags || [];

    note.noteId = this._store.note$.value.noteId;
    note.title = this.form.value.title;
    note.body = this.form.value.body;
    note.tags = tags.map(x => this._store.tags$.value.find(t => t.name == x));

    this._notesService
      .save({
        note
      })
      .pipe(takeUntil(this.onDestroy), tap(() => this._router.navigateByUrl('/notes')))
      .subscribe();
  }

  public get slug(): string {
    return this._activatedRoute.snapshot.params['slug'];
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }

  filterItems(itemName: string) {
    return this.items.filter(item => item.name.toLowerCase().indexOf(itemName.toLowerCase()) === 0);
  }

  handleChipClicked($event) {
    const tag = this._store.tags$.value.find(x => x.name == $event.item);
    this._router.navigate(['tags', tag.slug]);
  }
}
