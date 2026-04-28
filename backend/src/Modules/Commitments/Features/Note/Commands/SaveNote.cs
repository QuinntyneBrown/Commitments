using System.Text.RegularExpressions;
using Commitments.Data;
using Commitments.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoteEntity = Commitments.Domain.NoteAggregate.Note;

namespace Commitments.Features.Note;

public class SaveNoteCommandValidator : AbstractValidator<SaveNoteRequest>
{
    public SaveNoteCommandValidator()
    {
        RuleFor(r => r.Note.Title).NotEmpty();
    }
}

public class SaveNoteRequest : IRequest<SaveNoteResponse>
{
    public NoteDto Note { get; set; } = new();
}

public class SaveNoteResponse
{
    public Guid NoteId { get; set; }
    public string Slug { get; set; } = string.Empty;
}

public class SaveNoteCommandHandler : IRequestHandler<SaveNoteRequest, SaveNoteResponse>
{
    private static readonly Regex ScriptTag = new(@"<script\b[^>]*>.*?</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    private static readonly Regex EventHandler = new(@"\son\w+\s*=\s*(""[^""]*""|'[^']*'|[^\s>]+)", RegexOptions.IgnoreCase);

    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public SaveNoteCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task<SaveNoteResponse> Handle(SaveNoteRequest request, CancellationToken cancellationToken)
    {
        var note = await _context.Notes.FindAsync(new object?[] { request.Note.NoteId }, cancellationToken);
        if (note is null) _context.Notes.Add(note = new NoteEntity { ProfileId = request.Note.ProfileId });

        note.Title = request.Note.Title;
        note.Slug = await UniqueSlug(request.Note.Title.GenerateSlug(), note.ProfileId, note.NoteId, cancellationToken);
        note.Body = Sanitize(request.Note.Body);

        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new NoteSavedEvent { NoteId = note.NoteId, ProfileId = note.ProfileId }, cancellationToken);

        return new SaveNoteResponse { NoteId = note.NoteId, Slug = note.Slug };
    }

    private async Task<string> UniqueSlug(string baseSlug, Guid profileId, Guid noteId, CancellationToken ct)
    {
        var taken = await _context.Notes
            .Where(n => n.ProfileId == profileId && n.NoteId != noteId)
            .Select(n => n.Slug)
            .ToListAsync(ct);

        if (!taken.Contains(baseSlug)) return baseSlug;
        for (var i = 2; ; i++)
        {
            var candidate = $"{baseSlug}-{i}";
            if (!taken.Contains(candidate)) return candidate;
        }
    }

    private static string Sanitize(string body)
        => EventHandler.Replace(ScriptTag.Replace(body ?? string.Empty, string.Empty), string.Empty);
}
