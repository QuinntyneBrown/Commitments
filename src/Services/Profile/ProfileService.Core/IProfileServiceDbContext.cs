// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ProfileService.Core;
using Microsoft.EntityFrameworkCore;
using ProfileService.Core.AggregateModel.ProfileAggregate;

namespace ProfileService.Core;

public interface IProfileServiceDbContext
{
    DbSet<Profile> Profiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}


