using Commitments.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.Tag;

public class GetTagBySlugRequest : IRequest<GetTagBySlugResponse>
{
    public Guid ProfileId { get; set; }
    public string Slug { get; set; } = string.Empty;
}

public class GetTagBySlugResponse
{
    public TagDto? Tag { get; set; }
}

public class GetTagBySlugQueryHandler : IRequestHandler<GetTagBySlugRequest, GetTagBySlugResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetTagBySlugQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetTagBySlugResponse> Handle(GetTagBySlugRequest request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .SingleOrDefaultAsync(t => t.ProfileId == request.ProfileId && t.Slug == request.Slug, cancellationToken);

        return new GetTagBySlugResponse { Tag = tag is null ? null : TagDto.FromTag(tag) };
    }
}
