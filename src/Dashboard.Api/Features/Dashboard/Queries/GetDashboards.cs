using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.Dashboard;

public class GetDashboardsRequest : IRequest<GetDashboardsResponse> { }

public class GetDashboardsResponse : ResponseBase
{
    public required List<DashboardDto> Dashboards { get; set; }
}

public class GetDashboardsRequestHandler : IRequestHandler<GetDashboardsRequest, GetDashboardsResponse>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardsRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetDashboardsResponse> Handle(GetDashboardsRequest request, CancellationToken cancellationToken)
    {
        return new GetDashboardsResponse
        {
            Dashboards = await _context.Dashboards.ToDtosAsync(cancellationToken)
        };
    }
}
