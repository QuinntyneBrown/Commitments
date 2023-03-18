// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.AggregateModel;

public class User
{
    public User()
    {
        Dashboards = new List<Dashboard>();
    }
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public List<Dashboard> Dashboards { get; set; }
}
