// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.DashboardCardAggregate.Commands;

public class CreateDashboardCardRangeRequest : IRequest<CreateDashboardCardRangeResponse>
{
    public IEnumerable<DashboardCardDto> DashboardCards { get; set; }
}

public class CreateDashboardCardRangeResponse
{
    public IEnumerable<Guid> DashboardCardIds { get; set; }
}

public class CreateDashboardCardRangeCommandHandler : IRequestHandler<CreateDashboardCardRangeRequest, CreateDashboardCardRangeResponse>
{
    public IDashboardServiceDbContext _context { get; set; }
    public ISender _sender { get; set; }
    public CreateDashboardCardRangeCommandHandler(IDashboardServiceDbContext context, ISender sender)
    {
        _context = context;
        _sender = sender;
    }

    public async Task<CreateDashboardCardRangeResponse> Handle(CreateDashboardCardRangeRequest request, CancellationToken cancellationToken)
    {
        var dashboardCardIds = new List<Guid>();

        foreach (var dashboardCard in request.DashboardCards)
        {
            var response = await _sender.Send(new CreateDashboardCardRequest()
            {
                DashboardId = dashboardCard.DashboardId,
                CardId = dashboardCard.CardId,
                CardLayoutId = dashboardCard.CardLayoutId,
                Options = dashboardCard.Options
            });

            dashboardCardIds.Add(response.DashboardCard.DashboardCardId);
        }

        return new CreateDashboardCardRangeResponse()
        {
            DashboardCardIds = dashboardCardIds
        };
    }

}