// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Commitments.Core.AggregateModel.NoteAggregate;

namespace Commitments.Core.AggregateModel.TagAggregate;

public class Tag : BaseEntity
{
    public Guid TagId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}

