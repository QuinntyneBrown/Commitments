using Dashboard.Api.Features.Card;
using Dashboard.Api.Features.CardLayout;
using Microsoft.EntityFrameworkCore;
using DashboardCardModel = Dashboard.Core.Model.DashboardCardAggregate.DashboardCard;

namespace Dashboard.Api.Features.DashboardCard;

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
