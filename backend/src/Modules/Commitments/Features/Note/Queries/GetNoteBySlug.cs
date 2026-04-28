using Commitments.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.Note;

public class GetNoteBySlugRequest : IRequest<GetNoteBySlugResponse>
{
    public Guid ProfileId { get; set; }
    public string Slug { get; set; } = string.Empty;
}

public class GetNoteBySlugResponse
{
    public NoteDto? Note { get; set; }
}

public class GetNoteBySlugQueryHandler : IRequestHandler<GetNoteBySlugRequest, GetNoteBySlugResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetNoteBySlugQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetNoteBySlugResponse> Handle(GetNoteBySlugRequest request, CancellationToken cancellationToken)
    {
        var note = await _context.Notes
            .SingleOrDefaultAsync(n => n.ProfileId == request.ProfileId && n.Slug == request.Slug, cancellationToken);

        return new GetNoteBySlugResponse { Note = note is null ? null : NoteDto.FromNote(note) };
    }
}
