// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;

namespace DashboardService.Core.AggregateModel.DashboardCardAggregate.Commands;

public class UpdateDashboardCardRequestValidator : AbstractValidator<UpdateDashboardCardRequest> { }

public class UpdateDashboardCardRequest : IRequest<UpdateDashboardCardResponse>
{
    public Guid DashboardCardId { get; set; }
    public Guid DashboardId { get; set; }
    public Guid CardId { get; set; }
    public Guid CardLayoutId { get; set; }
    public DashboardDto Dashboard { get; set; }
    public CardDto Card { get; set; }
    public CardLayoutDto CardLayout { get; set; }
    public JObject Options { get; set; }
}


public class UpdateDashboardCardResponse : ResponseBase
{
    public DashboardCardDto DashboardCard { get; set; }
}


public class UpdateDashboardCardRequestHandler : IRequestHandler<UpdateDashboardCardRequest, UpdateDashboardCardResponse>
{
    private readonly ILogger<UpdateDashboardCardRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public UpdateDashboardCardRequestHandler(ILogger<UpdateDashboardCardRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateDashboardCardResponse> Handle(UpdateDashboardCardRequest request, CancellationToken cancellationToken)
    {
        var dashboardCard = await _context.DashboardCards.SingleAsync(x => x.DashboardCardId == request.DashboardCardId);

        dashboardCard.DashboardCardId = request.DashboardCardId;
        dashboardCard.DashboardId = request.DashboardId;
        dashboardCard.CardId = request.CardId;
        dashboardCard.CardLayoutId = request.CardLayoutId;
        dashboardCard.Options = request.Options;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            DashboardCard = dashboardCard.ToDto()
        };

    }

}