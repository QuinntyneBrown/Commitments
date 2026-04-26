using Commitments.Shared;
using Dashboard.Data;
using DashboardCardModel = Dashboard.Domain.DashboardCardAggregate.DashboardCard;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Features.DashboardCard;

public class SaveDashboardCardRangeRequest : IRequest<SaveDashboardCardRangeResponse>
{
    public List<DashboardCardDto> DashboardCards { get; set; }
    public Guid DashboardId { get; set; }
}

public class SaveDashboardCardRangeResponse : ResponseBase
{
    public List<Guid> DashboardCardIds { get; set; }
}

public class SaveDashboardCardRangeRequestHandler : IRequestHandler<SaveDashboardCardRangeRequest, SaveDashboardCardRangeResponse>
{
    private readonly IDashboardDbContext _context;

    public SaveDashboardCardRangeRequestHandler(IDashboardDbContext context) => _context = context;

    public async Task<SaveDashboardCardRangeResponse> Handle(SaveDashboardCardRangeRequest request, CancellationToken cancellationToken)
    {
        var existing = await _context.DashboardCards
            .Where(x => x.DashboardId == request.DashboardId)
            .ToListAsync(cancellationToken);

        _context.DashboardCards.RemoveRange(existing);

        var newCards = request.DashboardCards.Select(dto => new DashboardCardModel
        {
            DashboardId = request.DashboardId,
            CardId = dto.CardId,
            CardLayoutId = dto.CardLayoutId,
            Options = dto.Options
        }).ToList();

        _context.DashboardCards.AddRange(newCards);
        await _context.SaveChangesAsync(cancellationToken);

        return new SaveDashboardCardRangeResponse
        {
            DashboardCardIds = newCards.Select(x => x.DashboardCardId).ToList()
        };
    }
}
