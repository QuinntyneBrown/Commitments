using Commitments.Api.Features.Tags;
using Commitments.Core.Entities;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Api.Features.Notes;

public class NoteDto
{        
    public int NoteId { get; set; }
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
