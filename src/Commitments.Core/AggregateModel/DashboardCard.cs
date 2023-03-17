// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;


namespace Commitments.Core.AggregateModel;

public class DashboardCard: BaseEntity
{
    public int DashboardCardId { get; set; }
    [ForeignKey("Dashboard")]
    public int DashboardId { get; set; }
    [ForeignKey("Card")]
    public int? CardId { get; set; }
    [ForeignKey("CardLayout")]
    public int? CardLayoutId { get; set; }
    public string Options { get; set; }
    public Dashboard Dashboard { get; set; }
    public Card Card { get; set; }
    public CardLayout CardLayout { get; set; }
}

