// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Commitments.Core.AggregateModel.DashboardCardAggregate;

namespace Commitments.Core.AggregateModel.DashboardAggregate;

public class Dashboard : BaseEntity
{
    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid ProfileId { get; set; }
    public ICollection<DashboardCard> DashboardCards { get; set; }
        = new HashSet<DashboardCard>();
}

