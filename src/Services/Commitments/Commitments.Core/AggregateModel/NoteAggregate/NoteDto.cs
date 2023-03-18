// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.TagAggregate;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Core.AggregateModel.NoteAggregate;

public class NoteDto
{
    public Guid NoteId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public ICollection<TagDto> Tags = new HashSet<TagDto>();

    public static NoteDto FromNote(Note note, bool includeTags = true)
    {
        var model = new NoteDto
        {
            NoteId = note.NoteId,
            Title = note.Title,
            Slug = note.Slug,
            Body = note.Body
        };

        if (includeTags)
            model.Tags = note.NoteTags.Select(x => TagDto.FromTag(x.Tag)).ToList();

        return model;
    }
}

