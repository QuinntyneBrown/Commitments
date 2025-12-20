// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CardAggregate;
using Commitments.Core.Model.CardLayoutAggregate;
using Commitments.Core.Model.DashboardAggregate;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commitments.Core.Model.DashboardCardAggregate;

public class DashboardCard
{
    public Guid DashboardCardId { get; set; }
    [ForeignKey("Dashboard")]
    public Guid DashboardId { get; set; }
    [ForeignKey("Card")]
    public Guid CardId { get; set; }
    [ForeignKey("CardLayout")]
    public Guid CardLayoutId { get; set; }
    public Dashboard Dashboard { get; set; }
    public Card Card { get; set; }
    public CardLayout CardLayout { get; set; }
    public JObject Options { get; set; }
}