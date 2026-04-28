using Commitments.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.Note;

public class RemoveTagFromNoteRequest : IRequest
{
    public Guid NoteId { get; set; }
    public Guid TagId { get; set; }
}

public class RemoveTagFromNoteCommandHandler : IRequestHandler<RemoveTagFromNoteRequest>
{
    private readonly ICommitmentsDbContext _context;

    public RemoveTagFromNoteCommandHandler(ICommitmentsDbContext context) => _context = context;

    public async Task Handle(RemoveTagFromNoteRequest request, CancellationToken cancellationToken)
    {
        var join = await _context.NoteTags
            .SingleOrDefaultAsync(nt => nt.NoteId == request.NoteId && nt.TagId == request.TagId, cancellationToken);
        if (join is null) return;

        _context.NoteTags.Remove(join);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
