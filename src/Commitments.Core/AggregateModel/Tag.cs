// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;


namespace Commitments.Core.AggregateModel;

public class Tag: BaseEntity
{
    public int TagId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}

