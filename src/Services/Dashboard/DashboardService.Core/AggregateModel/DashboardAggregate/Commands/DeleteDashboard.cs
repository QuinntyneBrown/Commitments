// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.DashboardAggregate.Commands;

public class DeleteDashboardRequestValidator : AbstractValidator<DeleteDashboardRequest> { }

public class DeleteDashboardRequest : IRequest<DeleteDashboardResponse>
{
    public Guid DashboardId { get; set; }
}


public class DeleteDashboardResponse : ResponseBase
{
    public DashboardDto Dashboard { get; set; }
}


public class DeleteDashboardRequestHandler : IRequestHandler<DeleteDashboardRequest, DeleteDashboardResponse>
{
    private readonly ILogger<DeleteDashboardRequestHandler> _logger;

    private readonly IDashboardServiceDbContext _context;

    public DeleteDashboardRequestHandler(ILogger<DeleteDashboardRequestHandler> logger, IDashboardServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteDashboardResponse> Handle(DeleteDashboardRequest request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards.FindAsync(request.DashboardId);

        _context.Dashboards.Remove(dashboard);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Dashboard = dashboard.ToDto()
        };
    }

}