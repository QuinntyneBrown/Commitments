// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.Model.DashboardCardAggregate;
using Commitments.Core.Services.Kernel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using DashboardCardEntity = Commitments.Core.Model.DashboardCardAggregate.DashboardCard;

namespace Commitments.Api.Features.DashboardCard;

public class CreateDashboardCardRequestValidator : AbstractValidator<CreateDashboardCardRequest> { }

public class CreateDashboardCardRequest : IRequest<CreateDashboardCardResponse>
{
    public Guid DashboardId { get; set; }
    public Guid CardId { get; set; }
    public Guid CardLayoutId { get; set; }
    public JObject Options { get; set; }
}


public class CreateDashboardCardResponse : ResponseBase
{
    public DashboardCardDto DashboardCard { get; set; }
}


public class CreateDashboardCardRequestHandler : IRequestHandler<CreateDashboardCardRequest, CreateDashboardCardResponse>
{
    private readonly ILogger<CreateDashboardCardRequestHandler> _logger;

    private readonly ICommitmentsDbContext _context;

    public CreateDashboardCardRequestHandler(ILogger<CreateDashboardCardRequestHandler> logger, ICommitmentsDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateDashboardCardResponse> Handle(CreateDashboardCardRequest request, CancellationToken cancellationToken)
    {
        var dashboardCard = new DashboardCardEntity();

        _context.DashboardCards.Add(dashboardCard);

        dashboardCard.DashboardId = request.DashboardId;

        dashboardCard.CardId = request.CardId;

        dashboardCard.CardLayoutId = request.CardLayoutId;

        dashboardCard.Options = request.Options;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            DashboardCard = dashboardCard.ToDto()
        };

    }

}