// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DigitalAssetService.Core;
using Microsoft.EntityFrameworkCore;
using DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate;

namespace DigitalAssetService.Infrastructure.Data;

public class DigitalAssetServiceDbContext : DbContext, IDigitalAssetServiceDbContext
{
    public DigitalAssetServiceDbContext(DbContextOptions<DigitalAssetServiceDbContext> options) : base(options)
    {
    }

    public DbSet<DigitalAsset> DigitalAssets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DigitalAssets");

        base.OnModelCreating(modelBuilder);
    }
}


