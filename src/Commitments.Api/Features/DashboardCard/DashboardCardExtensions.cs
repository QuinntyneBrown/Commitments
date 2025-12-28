// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DashboardCardAggregate;
using Microsoft.EntityFrameworkCore;
using DashboardCardEntity = Commitments.Core.Model.DashboardCardAggregate.DashboardCard;

namespace Commitments.Api.Features.DashboardCard;

public static class DashboardCardExtensions
{
    public static DashboardCardDto ToDto(this DashboardCardEntity dashboardCard)
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

    public async static Task<List<DashboardCardDto>> ToDtosAsync(this IQueryable<DashboardCardEntity> dashboardCards, CancellationToken cancellationToken)
    {
        return await dashboardCards.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}