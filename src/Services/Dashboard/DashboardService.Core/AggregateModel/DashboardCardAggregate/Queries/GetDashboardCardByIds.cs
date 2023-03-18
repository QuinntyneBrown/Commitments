// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.DashboardCardAggregate.Queries;

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
    public IDashboardServiceDbContext _context { get; set; }
    public GetDashboardCardByIdsQueryHandler(IDashboardServiceDbContext context) => _context = context;

    public async Task<GetDashboardCardByIdsResponse> Handle(GetDashboardCardByIdsRequest request, CancellationToken cancellationToken)
        => new GetDashboardCardByIdsResponse()
        {
            DashboardCards = await _context.DashboardCards
            .Where(x => request.DashboardCardIds.Contains(x.DashboardCardId))
            .Select(x => x.ToDto()).ToListAsync()
        };
}

