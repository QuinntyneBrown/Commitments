// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.DashboardCardAggregate;
using Newtonsoft.Json;


namespace Commitments.Core.AggregateModel.DashboardCardAggregate;

public class DashboardCardDto
{
    public Guid DashboardCardId { get; set; }
    public Guid DashboardId { get; set; }
    public Guid? CardId { get; set; }
    public Guid? CardLayoutId { get; set; }
    public OptionsDto Options { get; set; }

    public static DashboardCardDto FromDashboardCard(DashboardCard dashboardCard)
        => new DashboardCardDto
        {
            DashboardCardId = dashboardCard.DashboardCardId,
            DashboardId = dashboardCard.DashboardId,
            CardId = dashboardCard.CardId,
            CardLayoutId = dashboardCard.CardLayoutId,
            Options = JsonConvert.DeserializeObject<OptionsDto>(dashboardCard.Options)
        };
}

