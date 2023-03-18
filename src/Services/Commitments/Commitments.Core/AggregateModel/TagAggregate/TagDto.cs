// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.NoteAggregate;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Core.AggregateModel.TagAggregate;

public class TagDto
{
    public Guid TagId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public ICollection<NoteDto> Notes { get; set; }
    public static TagDto FromTag(Tag tag)
        => new TagDto
        {
            TagId = tag.TagId,
            Name = tag.Name,
            Slug = tag.Slug,
            Notes = tag.NoteTags.Select(x => NoteDto.FromNote(x.Note, false)).ToList()
        };
}

