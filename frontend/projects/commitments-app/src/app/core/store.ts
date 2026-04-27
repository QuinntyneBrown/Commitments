// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { HubClient } from './hub-client';
import { Note } from '../models/note';
import { Tag } from '../models/tag';

export interface NoteSavedPayload { noteId: string; title: string; kind: 'Created' | 'Updated'; }
export interface NoteRemovedPayload { noteId: string; }
export interface TagSavedPayload { tagId: string; name: string; }
export interface TagRemovedPayload { tagId: string; }

@Injectable({ providedIn: 'root' })
export class Store {
  private readonly _hubClient: HubClient;

  public note$: BehaviorSubject<Note> = new BehaviorSubject(<Note>{});
  public notes$: BehaviorSubject<Array<Note>> = new BehaviorSubject([]);
  public tags$: BehaviorSubject<Array<Tag>> = new BehaviorSubject([]);

  constructor(hubClient: HubClient = inject(HubClient)) {
    this._hubClient = hubClient;
  }

  public handleTagSaved(payload: { tag: Tag }) {
    this.tags$.next([...this.tags$.value, payload.tag]);
  }

  public handleTagRemoved(payload: { tagId: number }) {
    const tags = this.tags$.value;
    const deletedTagIndex = tags.findIndex(x => x.tagId == payload.tagId);
    tags.splice(deletedTagIndex, 1);
    this.tags$.next([...tags]);
  }

  public get savedNotes$() {
    return this._hubClient.on<NoteSavedPayload>('noteSaved')
      .pipe(map(p => ({ note: { noteId: p.noteId, title: p.title } })));
  }

  public get removedNotes$() {
    return this._hubClient.on<NoteRemovedPayload>('noteRemoved')
      .pipe(map(p => ({ noteId: p.noteId })));
  }

  public get savedTags$() {
    return this._hubClient.on<TagSavedPayload>('tagSaved')
      .pipe(map(p => ({ tag: { tagId: p.tagId, name: p.name } })));
  }

  public get removedTags$() {
    return this._hubClient.on<TagRemovedPayload>('tagRemoved')
      .pipe(map(p => ({ tagId: p.tagId })));
  }
}
