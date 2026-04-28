namespace Commitments.Domain.NoteAggregate;

public class NoteTag
{
    public Guid NoteTagId { get; set; } = Guid.NewGuid();
    public Guid NoteId { get; set; }
    public Guid TagId { get; set; }
}
