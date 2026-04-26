using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.Dashboard;

public class GetDashboardByIdRequest : IRequest<GetDashboardByIdResponse>
{
    public Guid DashboardId { get; set; }
}

public class GetDashboardByIdResponse : ResponseBase
{
    public DashboardDto? Dashboard { get; set; }
}

public class GetDashboardByIdRequestHandler : IRequestHandler<GetDashboardByIdRequest, GetDashboardByIdResponse>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardByIdRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetDashboardByIdResponse> Handle(GetDashboardByIdRequest request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards.FindAsync(request.DashboardId);
        return new GetDashboardByIdResponse { Dashboard = dashboard?.ToDto() };
    }
}
