using Commitments.Data;
using Commitments.Shared;
using MediatR;

namespace Commitments.Features.Note;

public class RemoveNoteRequest : IRequest
{
    public Guid NoteId { get; set; }
}

public class RemoveNoteCommandHandler : IRequestHandler<RemoveNoteRequest>
{
    private readonly ICommitmentsDbContext _context;
    private readonly IEventBus _bus;

    public RemoveNoteCommandHandler(ICommitmentsDbContext context, IEventBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task Handle(RemoveNoteRequest request, CancellationToken cancellationToken)
    {
        var note = await _context.Notes.FindAsync(new object?[] { request.NoteId }, cancellationToken);
        if (note is null) return;

        var profileId = note.ProfileId;
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync(cancellationToken);

        await _bus.PublishAsync(new NoteRemovedEvent { NoteId = request.NoteId, ProfileId = profileId }, cancellationToken);
    }
}
