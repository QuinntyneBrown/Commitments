// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace Commitments.Api.Features.Dashboard;

public class GetDashboardByProfileIdRequest : IRequest<GetDashboardByProfileIdResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetDashboardByProfileIdResponse
{
    public DashboardDto Dashboard { get; set; }
}

public class GetDashboardByProfileIdQueryHandler : IRequestHandler<GetDashboardByProfileIdRequest, GetDashboardByProfileIdResponse>
{
    public ICommitmentsDbContext _context { get; set; }

    public GetDashboardByProfileIdQueryHandler(ICommitmentsDbContext context) => _context = context;

    public async Task<GetDashboardByProfileIdResponse> Handle(GetDashboardByProfileIdRequest request, CancellationToken cancellationToken)
        => new GetDashboardByProfileIdResponse()
        {
            Dashboard = (await _context.Dashboards
                .Include(x => x.DashboardCards)
                .SingleOrDefaultAsync(x => x.ProfileId == request.ProfileId))!.ToDto()
        };
}