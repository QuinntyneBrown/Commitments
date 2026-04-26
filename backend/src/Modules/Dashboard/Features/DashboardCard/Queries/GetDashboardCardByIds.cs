using Commitments.Shared;
using Dashboard.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Api.Features.DashboardCard;

public class GetDashboardCardByIdsRequest : IRequest<GetDashboardCardByIdsResponse>
{
    public List<Guid> DashboardCardIds { get; set; }
}

public class GetDashboardCardByIdsResponse : ResponseBase
{
    public required List<DashboardCardDto> DashboardCards { get; set; }
}

public class GetDashboardCardByIdsRequestHandler : IRequestHandler<GetDashboardCardByIdsRequest, GetDashboardCardByIdsResponse>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardCardByIdsRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetDashboardCardByIdsResponse> Handle(GetDashboardCardByIdsRequest request, CancellationToken cancellationToken)
    {
        return new GetDashboardCardByIdsResponse
        {
            DashboardCards = await _context.DashboardCards
                .Include(x => x.Card)
                .Include(x => x.CardLayout)
                .Where(x => request.DashboardCardIds.Contains(x.DashboardCardId))
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken)
        };
    }
}
