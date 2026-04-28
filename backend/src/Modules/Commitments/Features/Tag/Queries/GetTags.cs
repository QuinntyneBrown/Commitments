using Commitments.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Features.Tag;

public class GetTagsRequest : IRequest<GetTagsResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetTagsResponse
{
    public IEnumerable<TagDto> Tags { get; set; } = Array.Empty<TagDto>();
}

public class GetTagsQueryHandler : IRequestHandler<GetTagsRequest, GetTagsResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetTagsQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetTagsResponse> Handle(GetTagsRequest request, CancellationToken cancellationToken)
    {
        var rows = await _context.Tags
            .Where(t => t.ProfileId == request.ProfileId)
            .OrderBy(t => t.Name)
            .Select(t => TagDto.FromTag(t))
            .ToListAsync(cancellationToken);

        return new GetTagsResponse { Tags = rows };
    }
}
