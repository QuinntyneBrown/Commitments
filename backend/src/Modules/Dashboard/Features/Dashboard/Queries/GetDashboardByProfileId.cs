using Commitments.Shared;
using Dashboard.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Features.Dashboard;

public class GetDashboardByProfileIdRequest : IRequest<GetDashboardByProfileIdResponse>
{
    public Guid ProfileId { get; set; }
}

public class GetDashboardByProfileIdResponse : ResponseBase
{
    public required List<DashboardDto> Dashboards { get; set; }
}

public class GetDashboardByProfileIdRequestHandler : IRequestHandler<GetDashboardByProfileIdRequest, GetDashboardByProfileIdResponse>
{
    private readonly IDashboardDbContext _context;

    public GetDashboardByProfileIdRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<GetDashboardByProfileIdResponse> Handle(GetDashboardByProfileIdRequest request, CancellationToken cancellationToken)
    {
        return new GetDashboardByProfileIdResponse
        {
            Dashboards = await _context.Dashboards
                .Include(x => x.DashboardCards)
                .Where(x => x.ProfileId == request.ProfileId)
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken)
        };
    }
}
