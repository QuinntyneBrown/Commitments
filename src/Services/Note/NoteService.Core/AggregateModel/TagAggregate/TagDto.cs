// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.NoteAggregate;

namespace NoteService.Core.AggregateModel.TagAggregate;

public class TagDto
{
    public Guid TagId { get; set; }
    public Guid Name { get; set; }
    public string Slug { get; set; }
    public List<NoteDto> Notes { get; set; }
}