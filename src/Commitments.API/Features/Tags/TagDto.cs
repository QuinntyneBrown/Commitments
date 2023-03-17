using Commitments.Api.Features.Notes;
using Commitments.Core.AggregateModel;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Api.Features.Tags;

public class TagDto
{        
    public int TagId { get; set; }
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