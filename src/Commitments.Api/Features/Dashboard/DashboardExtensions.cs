// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DashboardAggregate;
using Microsoft.EntityFrameworkCore;
using DashboardEntity = Commitments.Core.Model.DashboardAggregate.Dashboard;

namespace Commitments.Api.Features.Dashboard;

public static class DashboardExtensions
{
    public static DashboardDto ToDto(this DashboardEntity dashboard)
    {
        return new DashboardDto
        {
            DashboardId = dashboard.DashboardId,
            Name = dashboard.Name,
            UserId = dashboard.UserId
        };

    }

    public async static Task<List<DashboardDto>> ToDtosAsync(this IQueryable<DashboardEntity> dashboards, CancellationToken cancellationToken)
    {
        return await dashboards.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }

}