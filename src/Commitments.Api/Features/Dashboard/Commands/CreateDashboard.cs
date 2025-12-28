// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.Model.DashboardAggregate;
using Commitments.Core.Model.DashboardCardAggregate;
using Commitments.Core.Services.Kernel;
using Commitments.Api.Features.DashboardCard;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using DashboardCardEntity = Commitments.Core.Model.DashboardCardAggregate.DashboardCard;
using DashboardEntity = Commitments.Core.Model.DashboardAggregate.Dashboard;

namespace Commitments.Api.Features.Dashboard;

public class CreateDashboardRequestValidator : AbstractValidator<CreateDashboardRequest> { }

public class CreateDashboardRequest : IRequest<CreateDashboardResponse>
{
    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? UserId { get; set; }
    public List<DashboardCardEntity> DashboardCards { get; set; }
}


public class CreateDashboardResponse : ResponseBase
{
    public DashboardDto Dashboard { get; set; }
}


public class CreateDashboardRequestHandler : IRequestHandler<CreateDashboardRequest, CreateDashboardResponse>
{
    private readonly ILogger<CreateDashboardRequestHandler> _logger;
    private readonly ICommitmentsDbContext _context;

    public CreateDashboardRequestHandler(ILogger<CreateDashboardRequestHandler> logger, ICommitmentsDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateDashboardResponse> Handle(CreateDashboardRequest request, CancellationToken cancellationToken)
    {
        var dashboard = new DashboardEntity(request.Name, request.UserId);

        _context.Dashboards.Add(dashboard);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Dashboard = dashboard.ToDto()
        };
    }
}