using Commitments.Shared;
using Dashboard.Data;
using MediatR;

namespace Dashboard.Features.Dashboard;

public class UpdateDashboardRequest : IRequest<UpdateDashboardResponse>
{
    public DashboardDto Dashboard { get; set; }
}

public class UpdateDashboardResponse : ResponseBase
{
    public DashboardDto Dashboard { get; set; }
}

public class UpdateDashboardRequestHandler : IRequestHandler<UpdateDashboardRequest, UpdateDashboardResponse>
{
    private readonly IDashboardDbContext _context;

    public UpdateDashboardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<UpdateDashboardResponse> Handle(UpdateDashboardRequest request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards.FindAsync(request.Dashboard.DashboardId);
        if (dashboard == null) return new UpdateDashboardResponse();

        dashboard.Name = request.Dashboard.Name;
        dashboard.ProfileId = request.Dashboard.ProfileId;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateDashboardResponse { Dashboard = dashboard.ToDto() };
    }
}
