// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CardAggregate;
using Commitments.Api.Features.Card;
using Commitments.Core.Model.CardLayoutAggregate;
using Commitments.Api.Features.CardLayout;
using Commitments.Core.Model.DashboardAggregate;
using Commitments.Api.Features.Dashboard;
using Newtonsoft.Json.Linq;

namespace Commitments.Api.Features.DashboardCard;

public class DashboardCardDto
{
    public Guid DashboardCardId { get; set; }
    public Guid DashboardId { get; set; }
    public Guid CardId { get; set; }
    public Guid CardLayoutId { get; set; }
    public DashboardDto Dashboard { get; set; }
    public CardDto Card { get; set; }
    public CardLayoutDto CardLayout { get; set; }
    public JObject Options { get; set; }
}