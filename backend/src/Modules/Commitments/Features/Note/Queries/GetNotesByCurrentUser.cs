using Commitments.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.Note;

public class GetNotesByCurrentUserRequest : IRequest<GetNotesByCurrentUserResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetNotesByCurrentUserResponse
{
    public IEnumerable<NoteDto> Notes { get; set; } = Array.Empty<NoteDto>();
}

public class GetNotesByCurrentUserQueryHandler : IRequestHandler<GetNotesByCurrentUserRequest, GetNotesByCurrentUserResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetNotesByCurrentUserQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetNotesByCurrentUserResponse> Handle(GetNotesByCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var rows = await _context.Notes
            .Where(n => n.ProfileId == request.ProfileId)
            .OrderByDescending(n => n.LastModifiedOn)
            .Select(n => NoteDto.FromNote(n))
            .ToListAsync(cancellationToken);

        return new GetNotesByCurrentUserResponse { Notes = rows };
    }
}
