// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.Services.Kernel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Commitments.Api.Features.DashboardCard;

public class DeleteDashboardCardRequestValidator : AbstractValidator<DeleteDashboardCardRequest> { }

public class DeleteDashboardCardRequest : IRequest<DeleteDashboardCardResponse>
{
    public Guid DashboardCardId { get; set; }
}


public class DeleteDashboardCardResponse : ResponseBase
{
    public DashboardCardDto DashboardCard { get; set; }
}


public class DeleteDashboardCardRequestHandler : IRequestHandler<DeleteDashboardCardRequest, DeleteDashboardCardResponse>
{
    private readonly ILogger<DeleteDashboardCardRequestHandler> _logger;

    private readonly ICommitmentsDbContext _context;

    public DeleteDashboardCardRequestHandler(ILogger<DeleteDashboardCardRequestHandler> logger, ICommitmentsDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteDashboardCardResponse> Handle(DeleteDashboardCardRequest request, CancellationToken cancellationToken)
    {
        var dashboardCard = await _context.DashboardCards.FindAsync(request.DashboardCardId);

        _context.DashboardCards.Remove(dashboardCard);

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            DashboardCard = dashboardCard.ToDto()
        };
    }

}