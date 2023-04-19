// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel.CardLayoutAggregate;

public class CardLayoutDto
{
    public Guid CardLayoutId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}