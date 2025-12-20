// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DashboardCardAggregate;
using Commitments.Api.Features.DashboardCard;

namespace Commitments.Api.Features.Dashboard;

public class DashboardDto
{
    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? UserId { get; set; }
    public List<DashboardCardDto> DashboardCards { get; set; }
}