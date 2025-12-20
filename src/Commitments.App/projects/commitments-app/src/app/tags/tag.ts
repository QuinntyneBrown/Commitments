// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Note } from '../notes/note';

export class Tag {
  public tagId: any;

  public name: string;

  public slug: string;

  public notes?: Array<Note> = [];
}

