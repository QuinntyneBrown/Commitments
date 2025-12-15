// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CardAggregate;
using Commitments.Core.AggregateModel.CardLayoutAggregate;
using Commitments.Core.AggregateModel.DashboardAggregate;
using Newtonsoft.Json.Linq;

namespace Commitments.Core.AggregateModel.DashboardCardAggregate;

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