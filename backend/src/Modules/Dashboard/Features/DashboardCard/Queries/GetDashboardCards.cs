using Commitments.Shared;
using Dashboard.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Api.Features.DashboardCard;

public class GetDashboardCardsRequest : IRequest<GetDashboardCardsResponse> { }

public class GetDashboardCardsResponse : ResponseBase
{
    public required List<DashboardCardDto> DashboardCards { get; set; }
}

public class GetDashboardCardsRequestHandler : IRequestHandler<GetDashboardCardsRequest, GetDashboardCardsResponse>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardCardsRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetDashboardCardsResponse> Handle(GetDashboardCardsRequest request, CancellationToken cancellationToken)
    {
        return new GetDashboardCardsResponse
        {
            DashboardCards = await _context.DashboardCards
                .Include(x => x.Card)
                .Include(x => x.CardLayout)
                .ToDtosAsync(cancellationToken)
        };
    }
}
