using Commitments.Data;
using Commitments.Domain.NoteAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.Note;

public class AddTagToNoteRequest : IRequest
{
    public Guid NoteId { get; set; }
    public Guid TagId { get; set; }
}

public class AddTagToNoteCommandHandler : IRequestHandler<AddTagToNoteRequest>
{
    private readonly ICommitmentsDbContext _context;

    public AddTagToNoteCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(AddTagToNoteRequest request, CancellationToken cancellationToken)
    {
        var exists = await _context.NoteTags
            .AnyAsync(nt => nt.NoteId == request.NoteId && nt.TagId == request.TagId, cancellationToken);
        if (exists) return;

        _context.NoteTags.Add(new NoteTag { NoteId = request.NoteId, TagId = request.TagId });
        await _context.SaveChangesAsync(cancellationToken);
    }
}
