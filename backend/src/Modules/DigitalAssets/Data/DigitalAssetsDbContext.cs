using Commitments.Shared;
using DigitalAssets.Core;
using DigitalAssets.Core.Model.DigitalAssetAggregate;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssets.Infrastructure.Data;

public class DigitalAssetsDbContext : BaseDbContext, IDigitalAssetsDbContext
{
    public DigitalAssetsDbContext(DbContextOptions<DigitalAssetsDbContext> options)
        : base(options)
    {
    }

    public DbSet<DigitalAsset> DigitalAssets { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DigitalAssets");
        base.OnModelCreating(modelBuilder);
    }
}
