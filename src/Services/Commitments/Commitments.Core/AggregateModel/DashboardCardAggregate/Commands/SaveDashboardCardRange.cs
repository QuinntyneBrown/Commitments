// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;


namespace Commitments.Core.AggregateModel.DashboardCardAggregate.Commands;

public class SaveDashboardCardRangeRequest : IRequest<SaveDashboardCardRangeResponse>
{
    public IEnumerable<DashboardCardDto> DashboardCards { get; set; }
}

public class SaveDashboardCardRangeResponse
{
    public IEnumerable<Guid> DashboardCardIds { get; set; }
}

public class SaveDashboardCardRangeCommandHandler : IRequestHandler<SaveDashboardCardRangeRequest, SaveDashboardCardRangeResponse>
{
    public ICommimentsDbContext _context { get; set; }
    public IMediator _mediator { get; set; }
    public SaveDashboardCardRangeCommandHandler(ICommimentsDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<SaveDashboardCardRangeResponse> Handle(SaveDashboardCardRangeRequest request, CancellationToken cancellationToken)
    {
        var dashboardCardIds = new List<Guid>();

        foreach (var dashboardCard in request.DashboardCards)
        {
            var response = await _mediator.Send(new SaveDashboardCardRequest() { DashboardCard = dashboardCard });
            dashboardCardIds.Add(response.DashboardCardId);
        }

        return new SaveDashboardCardRangeResponse()
        {
            DashboardCardIds = dashboardCardIds
        };
    }

}

