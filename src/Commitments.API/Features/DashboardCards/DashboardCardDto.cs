// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using Newtonsoft.Json;


namespace Commitments.Api.Features.DashboardCards;

public class DashboardCardDto {
    public int DashboardCardId { get; set; }
    public int DashboardId { get; set; }
    public int? CardId { get; set; }
    public int? CardLayoutId { get; set; }
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

