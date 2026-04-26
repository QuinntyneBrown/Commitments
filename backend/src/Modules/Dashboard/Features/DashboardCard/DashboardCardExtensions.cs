using Dashboard.Features.Card;
using Dashboard.Features.CardLayout;
using Microsoft.EntityFrameworkCore;
using DashboardCardModel = Dashboard.Domain.DashboardCardAggregate.DashboardCard;

namespace Dashboard.Features.DashboardCard;

public static class DashboardCardExtensions
{
    public static DashboardCardDto ToDto(this DashboardCardModel dashboardCard)
    {
        return new DashboardCardDto
        {
            DashboardCardId = dashboardCard.DashboardCardId,
            DashboardId = dashboardCard.DashboardId,
            CardId = dashboardCard.CardId,
            CardLayoutId = dashboardCard.CardLayoutId,
            Card = dashboardCard.Card?.ToDto(),
            CardLayout = dashboardCard.CardLayout?.ToDto(),
            Options = dashboardCard.Options
        };
    }

    public static async Task<List<DashboardCardDto>> ToDtosAsync(this IQueryable<DashboardCardModel> dashboardCards, CancellationToken cancellationToken)
    {
        return await dashboardCards.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
