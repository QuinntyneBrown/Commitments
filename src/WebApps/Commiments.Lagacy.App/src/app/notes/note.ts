// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

import { Tag } from '../tags/tag';

export class Note {
  public noteId: any = 0;
  public title: string;
  public body: string;
  public tags?: Array<Tag> = [];
}

