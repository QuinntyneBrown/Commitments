using Identity.Domain.UserAggregate;
using Identity.Domain.ProfileAggregate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data;

public interface IIdentityDbContext : IDisposable
{
    DbSet<User> Users { get; }
    DbSet<Profile> Profiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
