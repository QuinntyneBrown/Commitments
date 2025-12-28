// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.Model.DashboardCardAggregate;
using Commitments.Core.Services.Kernel;
using Commitments.Api.Features.DashboardCard;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Commitments.Api.Features.Dashboard;

public class UpdateDashboardRequestValidator : AbstractValidator<UpdateDashboardRequest> { }

public class UpdateDashboardRequest : IRequest<UpdateDashboardResponse>
{
    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? UserId { get; set; }
    public List<DashboardCardDto> DashboardCards { get; set; }
}


public class UpdateDashboardResponse : ResponseBase
{
    public DashboardDto Dashboard { get; set; }
}


public class UpdateDashboardRequestHandler : IRequestHandler<UpdateDashboardRequest, UpdateDashboardResponse>
{
    private readonly ILogger<UpdateDashboardRequestHandler> _logger;

    private readonly ICommitmentsDbContext _context;

    public UpdateDashboardRequestHandler(ILogger<UpdateDashboardRequestHandler> logger, ICommitmentsDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateDashboardResponse> Handle(UpdateDashboardRequest request, CancellationToken cancellationToken)
    {
        var dashboard = await _context.Dashboards.SingleAsync(x => x.DashboardId == request.DashboardId);

        dashboard.DashboardId = request.DashboardId;
        dashboard.Name = request.Name;
        dashboard.UserId = request.UserId;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Dashboard = dashboard.ToDto()
        };

    }

}