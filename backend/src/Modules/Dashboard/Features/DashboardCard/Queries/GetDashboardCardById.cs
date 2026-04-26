using Commitments.Shared;
using Dashboard.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Api.Features.DashboardCard;

public class GetDashboardCardByIdRequest : IRequest<GetDashboardCardByIdResponse>
{
    public Guid DashboardCardId { get; set; }
}

public class GetDashboardCardByIdResponse : ResponseBase
{
    public DashboardCardDto? DashboardCard { get; set; }
}

public class GetDashboardCardByIdRequestHandler : IRequestHandler<GetDashboardCardByIdRequest, GetDashboardCardByIdResponse>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardCardByIdRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetDashboardCardByIdResponse> Handle(GetDashboardCardByIdRequest request, CancellationToken cancellationToken)
    {
        var dashboardCard = await _context.DashboardCards
            .Include(x => x.Card)
            .Include(x => x.CardLayout)
            .FirstOrDefaultAsync(x => x.DashboardCardId == request.DashboardCardId, cancellationToken);

        return new GetDashboardCardByIdResponse { DashboardCard = dashboardCard?.ToDto() };
    }
}
