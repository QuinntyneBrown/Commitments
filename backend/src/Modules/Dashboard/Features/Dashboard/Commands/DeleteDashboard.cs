using Commitments.Shared;
using Dashboard.Data;
using MediatR;

namespace Dashboard.Features.Dashboard;

public class DeleteDashboardRequest : IRequest<DeleteDashboardResponse>
{
    public Guid DashboardId { get; set; }
}

public class DeleteDashboardResponse : ResponseBase { }

public class DeleteDashboardRequestHandler : IRequestHandler<DeleteDashboardRequest, DeleteDashboardResponse>
{
    private readonly IDashboardDbContext _context;

    public DeleteDashboardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<DeleteDashboardResponse> Handle(DeleteDashboardRequest request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards.FindAsync(request.DashboardId);
        if (dashboard != null)
        {
            _context.Dashboards.Remove(dashboard);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return new DeleteDashboardResponse();
    }
}
