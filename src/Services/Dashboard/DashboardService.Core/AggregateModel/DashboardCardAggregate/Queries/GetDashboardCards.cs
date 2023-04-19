// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.DashboardCardAggregate.Queries;

public class GetDashboardCardsRequest : IRequest<GetDashboardCardsResponse> { }

public class GetDashboardCardsResponse : ResponseBase
{
    public List<DashboardCardDto> DashboardCards { get; set; }
}


public class GetDashboardCardsRequestHandler : IRequestHandler<GetDashboardCardsRequest, GetDashboardCardsResponse>
{
    private readonly ILogger<GetDashboardCardsRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public GetDashboardCardsRequestHandler(ILogger<GetDashboardCardsRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDashboardCardsResponse> Handle(GetDashboardCardsRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            DashboardCards = await _context.DashboardCards.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}