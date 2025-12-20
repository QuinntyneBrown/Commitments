// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.DashboardCardAggregate;

namespace Commitments.Core.AggregateModel.DashboardAggregate;

public class DashboardDto
{
    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? UserId { get; set; }
    public List<DashboardCardDto> DashboardCards { get; set; }
}