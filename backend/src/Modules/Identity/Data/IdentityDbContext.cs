using Commitments.Shared;
using Identity.Data;
using Identity.Domain.UserAggregate;
using Identity.Domain.ProfileAggregate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public class IdentityDbContext : BaseDbContext, IIdentityDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; private set; }
    public DbSet<Profile> Profiles { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Identity");

        modelBuilder.Entity<User>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Profile>()
            .HasQueryFilter(e => !e.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}
