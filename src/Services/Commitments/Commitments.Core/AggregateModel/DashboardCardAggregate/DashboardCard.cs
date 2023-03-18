// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;
using Commitments.Core.AggregateModel.CardAggregate;
using Commitments.Core.AggregateModel.CardLayoutAggregate;
using Commitments.Core.AggregateModel.DashboardAggregate;

namespace Commitments.Core.AggregateModel.DashboardCardAggregate;

public class DashboardCard : BaseEntity
{
    public Guid DashboardCardId { get; set; }

    [ForeignKey("Dashboard")]
    public Guid DashboardId { get; set; }

    [ForeignKey("Card")]
    public Guid? CardId { get; set; }

    [ForeignKey("CardLayout")]

    public Guid? CardLayoutId { get; set; }

    public string Options { get; set; }

    public Dashboard Dashboard { get; set; }

    public Card Card { get; set; }

    public CardLayout CardLayout { get; set; }
}

