// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.AggregateModel.DashboardAggregate.Queries;

public class GetDashboardByIdRequest : IRequest<GetDashboardByIdResponse>
{
    public Guid DashboardId { get; set; }
}


public class GetDashboardByIdResponse : ResponseBase
{
    public DashboardDto Dashboard { get; set; }
}


public class GetDashboardByIdRequestHandler : IRequestHandler<GetDashboardByIdRequest, GetDashboardByIdResponse>
{
    private readonly ILogger<GetDashboardByIdRequestHandler> _logger;

    private readonly ICommitmentsDbContext _context;

    public GetDashboardByIdRequestHandler(ILogger<GetDashboardByIdRequestHandler> logger, ICommitmentsDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDashboardByIdResponse> Handle(GetDashboardByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Dashboard = (await _context.Dashboards.AsNoTracking().SingleOrDefaultAsync(x => x.DashboardId == request.DashboardId)).ToDto()
        };

    }

}