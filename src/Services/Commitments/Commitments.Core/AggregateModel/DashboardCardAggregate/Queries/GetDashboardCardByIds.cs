// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.DashboardCardAggregate.Queries;

public class GetDashboardCardByIdsRequest : IRequest<GetDashboardCardByIdsResponse>
{
    public Guid[] DashboardCardIds { get; set; }
}

public class GetDashboardCardByIdsResponse
{
    public IEnumerable<DashboardCardDto> DashboardCards { get; set; }
}

public class GetDashboardCardByIdsQueryHandler : IRequestHandler<GetDashboardCardByIdsRequest, GetDashboardCardByIdsResponse>
{
    public ICommimentsDbContext _context { get; set; }
    public GetDashboardCardByIdsQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetDashboardCardByIdsResponse> Handle(GetDashboardCardByIdsRequest request, CancellationToken cancellationToken)
        => new GetDashboardCardByIdsResponse()
        {
            DashboardCards = await _context.DashboardCards
            .Where(x => request.DashboardCardIds.Contains(x.DashboardCardId))
            .Select(x => DashboardCardDto.FromDashboardCard(x)).ToListAsync()
        };
}

