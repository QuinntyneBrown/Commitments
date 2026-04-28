using NoteEntity = Commitments.Domain.NoteAggregate.Note;

namespace Commitments.Features.Note;

public class NoteDto
{
    public Guid NoteId { get; set; }
    public Guid ProfileId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime LastModifiedOn { get; set; }

    public static NoteDto FromNote(NoteEntity note) => new()
    {
        NoteId = note.NoteId,
        ProfileId = note.ProfileId,
        Title = note.Title,
        Slug = note.Slug,
        Body = note.Body,
        LastModifiedOn = note.LastModifiedOn
    };
}
