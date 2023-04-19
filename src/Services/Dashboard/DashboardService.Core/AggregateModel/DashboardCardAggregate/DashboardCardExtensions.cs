// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.DashboardCardAggregate;

public static class DashboardCardExtensions
{
    public static DashboardCardDto ToDto(this DashboardCard dashboardCard)
    {
        return new DashboardCardDto
        {
            DashboardCardId = dashboardCard.DashboardCardId,
            DashboardId = dashboardCard.DashboardId,
            CardId = dashboardCard.CardId,
            CardLayoutId = dashboardCard.CardLayoutId,
            Options = dashboardCard.Options,
        };

    }

    public async static Task<List<DashboardCardDto>> ToDtosAsync(this IQueryable<DashboardCard> dashboardCards, CancellationToken cancellationToken)
    {
        return await dashboardCards.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}