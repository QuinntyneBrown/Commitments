using Commitments.Shared;
using Dashboard.Data;
using MediatR;

namespace Dashboard.Features.CardLayout;

public class GetCardLayoutsRequest : IRequest<GetCardLayoutsResponse> { }

public class GetCardLayoutsResponse : ResponseBase
{
    public required List<CardLayoutDto> CardLayouts { get; set; }
}

public class GetCardLayoutsRequestHandler : IRequestHandler<GetCardLayoutsRequest, GetCardLayoutsResponse>
{
    private readonly IDashboardDbContext _context;

    public GetCardLayoutsRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetCardLayoutsResponse> Handle(GetCardLayoutsRequest request, CancellationToken cancellationToken)
    {
        return new GetCardLayoutsResponse
        {
            CardLayouts = await _context.CardLayouts.ToDtosAsync(cancellationToken)
        };
    }
}
