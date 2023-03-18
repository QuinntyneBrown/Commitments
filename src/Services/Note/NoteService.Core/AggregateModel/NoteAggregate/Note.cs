// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.TagAggregate;

namespace NoteService.Core.AggregateModel.NoteAggregate;

public class Note
{
    public Guid NoteId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public List<Tag> Tags { get; set; }
}


