// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.AggregateModel.DashboardCardAggregate.Queries;

public class GetDashboardCardByIdRequest : IRequest<GetDashboardCardByIdResponse>
{
    public Guid DashboardCardId { get; set; }
}


public class GetDashboardCardByIdResponse : ResponseBase
{
    public DashboardCardDto DashboardCard { get; set; }
}


public class GetDashboardCardByIdRequestHandler : IRequestHandler<GetDashboardCardByIdRequest, GetDashboardCardByIdResponse>
{
    private readonly ILogger<GetDashboardCardByIdRequestHandler> _logger;

    private readonly ICommitmentsDbContext _context;

    public GetDashboardCardByIdRequestHandler(ILogger<GetDashboardCardByIdRequestHandler> logger, ICommitmentsDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDashboardCardByIdResponse> Handle(GetDashboardCardByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            DashboardCard = (await _context.DashboardCards.AsNoTracking().SingleOrDefaultAsync(x => x.DashboardCardId == request.DashboardCardId)).ToDto()
        };

    }

}