// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DashboardCardAggregate;
using Commitments.Core.Model.UserAggregate;

namespace Commitments.Core.Model.DashboardAggregate;

public class Dashboard
{
    public Dashboard(string name, Guid? userId = null)
    {
        Name = name;
        UserId = userId;
        DashboardCards = new List<DashboardCard>();
    }

    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ProfileId { get; set; }
    public User? User { get; set; }
    public List<DashboardCard> DashboardCards { get; set; }
}