using Commitments.Shared;
using Dashboard.Core;
using DashboardCardModel = Dashboard.Core.Model.DashboardCardAggregate.DashboardCard;
using MediatR;

namespace Dashboard.Api.Features.DashboardCard;

public class CreateDashboardCardRequest : IRequest<CreateDashboardCardResponse>
{
    public DashboardCardDto DashboardCard { get; set; }
}

public class CreateDashboardCardResponse : ResponseBase
{
    public DashboardCardDto DashboardCard { get; set; }
}

public class CreateDashboardCardRequestHandler : IRequestHandler<CreateDashboardCardRequest, CreateDashboardCardResponse>
{
    private readonly IDashboardDbContext _context;

    public CreateDashboardCardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<CreateDashboardCardResponse> Handle(CreateDashboardCardRequest request, CancellationToken cancellationToken)
    {
        var dashboardCard = new DashboardCardModel
        {
            DashboardId = request.DashboardCard.DashboardId,
            CardId = request.DashboardCard.CardId,
            CardLayoutId = request.DashboardCard.CardLayoutId,
            Options = request.DashboardCard.Options
        };

        _context.DashboardCards.Add(dashboardCard);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateDashboardCardResponse { DashboardCard = dashboardCard.ToDto() };
    }
}
