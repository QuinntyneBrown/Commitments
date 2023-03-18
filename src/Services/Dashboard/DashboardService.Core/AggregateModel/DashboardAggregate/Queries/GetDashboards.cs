// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.DashboardAggregate.Queries;

public class GetDashboardsRequest : IRequest<GetDashboardsResponse> { }

public class GetDashboardsResponse : ResponseBase
{
    public List<DashboardDto> Dashboards { get; set; }
}


public class GetDashboardsRequestHandler : IRequestHandler<GetDashboardsRequest, GetDashboardsResponse>
{
    private readonly ILogger<GetDashboardsRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public GetDashboardsRequestHandler(ILogger<GetDashboardsRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDashboardsResponse> Handle(GetDashboardsRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Dashboards = await _context.Dashboards.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}



