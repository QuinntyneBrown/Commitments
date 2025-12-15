// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { TagsModule } from '../tags/tags.module';
import { NotesService } from './notes.service';
import { CoreModule } from '../core/core.module';
import { NotesPageComponent } from 'notes-page/notes-page.component';

import { EditNotePageComponent } from 'edit-note-page/edit-note-page.component';
import { SharedModule } from '../shared/shared.module';
import { NoteResolver } from './note-resolver.service';
import { NotesByTagPageComponent } from 'notes-by-tag-page/notes-by-tag-page.component';

const declarations = [EditNotePageComponent, NotesPageComponent, NotesByTagPageComponent];

const providers = [NotesService, NoteResolver];

@NgModule({
  declarations: declarations,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule,

    CoreModule,
    SharedModule,
    TagsModule
  ],
  providers
})
export class NotesModule {}

