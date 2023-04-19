// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ProfileService.Core;
using Microsoft.EntityFrameworkCore;
using ProfileService.Core.AggregateModel.ProfileAggregate;

namespace ProfileService.Infrastructure.Data;

public class ProfileServiceDbContext : DbContext, IProfileServiceDbContext
{
    public ProfileServiceDbContext(DbContextOptions<ProfileServiceDbContext> options) : base(options)
    {
    }

    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Profile");

        base.OnModelCreating(modelBuilder);
    }
}