using Commitments.Shared;

namespace Commitments.Domain.NoteAggregate;

public class Note : BaseEntity
{
    public Guid NoteId { get; set; }
    public Guid ProfileId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
