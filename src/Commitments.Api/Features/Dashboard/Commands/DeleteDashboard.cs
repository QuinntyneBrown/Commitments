// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.Services.Kernel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Commitments.Api.Features.Dashboard;

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

    private readonly ICommitmentsDbContext _context;

    public DeleteDashboardRequestHandler(ILogger<DeleteDashboardRequestHandler> logger, ICommitmentsDbContext context)
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