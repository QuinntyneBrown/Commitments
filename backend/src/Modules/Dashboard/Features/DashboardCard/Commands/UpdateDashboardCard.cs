using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.DashboardCard;

public class UpdateDashboardCardRequest : IRequest<UpdateDashboardCardResponse>
{
    public DashboardCardDto DashboardCard { get; set; }
}

public class UpdateDashboardCardResponse : ResponseBase
{
    public DashboardCardDto DashboardCard { get; set; }
}

public class UpdateDashboardCardRequestHandler : IRequestHandler<UpdateDashboardCardRequest, UpdateDashboardCardResponse>
{
    private readonly IDashboardDbContext _context;

    public UpdateDashboardCardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<UpdateDashboardCardResponse> Handle(UpdateDashboardCardRequest request, CancellationToken cancellationToken)
    {
        var dashboardCard = await _context.DashboardCards.FindAsync(request.DashboardCard.DashboardCardId);
        if (dashboardCard == null) return new UpdateDashboardCardResponse();

        dashboardCard.DashboardId = request.DashboardCard.DashboardId;
        dashboardCard.CardId = request.DashboardCard.CardId;
        dashboardCard.CardLayoutId = request.DashboardCard.CardLayoutId;
        dashboardCard.Options = request.DashboardCard.Options;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateDashboardCardResponse { DashboardCard = dashboardCard.ToDto() };
    }
}
