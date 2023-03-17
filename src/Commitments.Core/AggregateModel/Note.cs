using System.Collections.Generic;


namespace Commitments.Core.AggregateModel;

public class Note: BaseEntity
{
    public int NoteId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
}
