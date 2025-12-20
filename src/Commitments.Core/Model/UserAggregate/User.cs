// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DashboardAggregate;

namespace Commitments.Core.Model.UserAggregate;

public class User : BaseEntity
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public ICollection<Dashboard> Dashboards { get; set; } = new HashSet<Dashboard>();
}
