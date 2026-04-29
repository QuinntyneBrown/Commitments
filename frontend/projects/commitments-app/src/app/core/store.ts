// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Injectable, inject, signal, WritableSignal } from '@angular/core';
import { map } from 'rxjs/operators';

import { HubClient } from './hub-client';
import { Note, Tag } from '@commitments/notes-feature';

export interface NoteSavedPayload { noteId: string; title: string; kind: 'Created' | 'Updated'; }
export interface NoteRemovedPayload { noteId: string; }
export interface TagSavedPayload { tagId: string; name: string; }
export interface TagRemovedPayload { tagId: string; }

@Injectable({ providedIn: 'root' })
export class Store {
  private readonly _hubClient: HubClient;

  public readonly note: WritableSignal<Note> = signal(<Note>{});
  public readonly notes: WritableSignal<Array<Note>> = signal([]);
  public readonly tags: WritableSignal<Array<Tag>> = signal([]);

  constructor(hubClient: HubClient = inject(HubClient)) {
    this._hubClient = hubClient;
  }

  public handleTagSaved(payload: { tag: Tag }) {
    this.tags.update(tags => [...tags, payload.tag]);
  }

  public handleTagRemoved(payload: { tagId: number }) {
    this.tags.update(tags => tags.filter(x => x.tagId != payload.tagId));
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
