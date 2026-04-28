using Commitments.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.Note;

public class GetNotesByTagSlugRequest : IRequest<GetNotesByTagSlugResponse>
{
    public Guid ProfileId { get; set; }
    public string Slug { get; set; } = string.Empty;
}

public class GetNotesByTagSlugResponse
{
    public IEnumerable<NoteDto> Notes { get; set; } = Array.Empty<NoteDto>();
}

public class GetNotesByTagSlugQueryHandler : IRequestHandler<GetNotesByTagSlugRequest, GetNotesByTagSlugResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetNotesByTagSlugQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetNotesByTagSlugResponse> Handle(GetNotesByTagSlugRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .SingleOrDefaultAsync(t => t.ProfileId == request.ProfileId && t.Slug == request.Slug, cancellationToken);
        if (tag is null) return new GetNotesByTagSlugResponse();

        var noteIds = _context.NoteTags
            .Where(nt => nt.TagId == tag.TagId)
            .Select(nt => nt.NoteId);

        var rows = await _context.Notes
            .Where(n => n.ProfileId == request.ProfileId && noteIds.Contains(n.NoteId))
            .OrderByDescending(n => n.LastModifiedOn)
            .Select(n => NoteDto.FromNote(n))
            .ToListAsync(cancellationToken);

        return new GetNotesByTagSlugResponse { Notes = rows };
    }
}
