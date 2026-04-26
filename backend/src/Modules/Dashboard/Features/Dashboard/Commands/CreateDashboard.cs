using Commitments.Shared;
using Dashboard.Data;
using Dashboard.Domain.DashboardAggregate;
using MediatR;

namespace Dashboard.Features.Dashboard;

public class CreateDashboardRequest : IRequest<CreateDashboardResponse>
{
    public DashboardDto Dashboard { get; set; }
}

public class CreateDashboardResponse : ResponseBase
{
    public DashboardDto Dashboard { get; set; }
}

public class CreateDashboardRequestHandler : IRequestHandler<CreateDashboardRequest, CreateDashboardResponse>
{
    private readonly IDashboardDbContext _context;

    public CreateDashboardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<CreateDashboardResponse> Handle(CreateDashboardRequest request, CancellationToken cancellationToken)
    {
        var dashboard = new DashboardEntity(request.Dashboard.Name, request.Dashboard.ProfileId);

        _context.Dashboards.Add(dashboard);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateDashboardResponse { Dashboard = dashboard.ToDto() };
    }
}
