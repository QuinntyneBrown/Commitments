// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Commitments.Core.AggregateModel.TagAggregate;

namespace Commitments.Core.AggregateModel.NoteAggregate;

public class NoteTag : BaseEntity
{
    public Guid NoteTagId { get; set; }
    public Guid NoteId { get; set; }
    public Guid TagId { get; set; }
    public Note Note { get; set; }
    public Tag Tag { get; set; }
}

