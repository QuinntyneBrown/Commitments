using Commitments.Shared;
using Dashboard.Core;
using MediatR;

namespace Dashboard.Api.Features.DashboardCard;

public class DeleteDashboardCardRequest : IRequest<DeleteDashboardCardResponse>
{
    public Guid DashboardCardId { get; set; }
}

public class DeleteDashboardCardResponse : ResponseBase { }

public class DeleteDashboardCardRequestHandler : IRequestHandler<DeleteDashboardCardRequest, DeleteDashboardCardResponse>
{
    private readonly IDashboardDbContext _context;

    public DeleteDashboardCardRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<DeleteDashboardCardResponse> Handle(DeleteDashboardCardRequest request, CancellationToken cancellationToken)
    {
        var dashboardCard = await _context.DashboardCards.FindAsync(request.DashboardCardId);
        if (dashboardCard != null)
        {
            _context.DashboardCards.Remove(dashboardCard);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return new DeleteDashboardCardResponse();
    }
}
