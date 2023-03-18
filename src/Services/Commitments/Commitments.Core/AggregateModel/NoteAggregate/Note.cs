// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;


namespace Commitments.Core.AggregateModel.NoteAggregate;

public class Note : BaseEntity
{
    public Guid NoteId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}

