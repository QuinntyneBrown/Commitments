// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Features.DashboardCards;
using Commitments.Core.AggregateModel;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Api.Features.Dashboards;

public class DashboardDto
{        
    public int DashboardId { get; set; }
    public string Name { get; set; }
    public int ProfileId { get; set; }
    public ICollection<DashboardCardDto> DashboardCards { get; set; }
    = new HashSet<DashboardCardDto>();

    public static DashboardDto FromDashboard(Dashboard dashboard)
        => new DashboardDto
        {
            DashboardId = dashboard.DashboardId,
            Name = dashboard.Name,
            ProfileId = dashboard.ProfileId,
            DashboardCards = dashboard.DashboardCards.Select(x => DashboardCardDto.FromDashboardCard(x)).ToList()
        };
}

