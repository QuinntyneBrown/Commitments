using Identity.Core.Model.UserAggregate;
using Identity.Core.Model.ProfileAggregate;
using Microsoft.EntityFrameworkCore;

namespace Identity.Core;

public interface IIdentityDbContext : IDisposable
{
    DbSet<User> Users { get; }
    DbSet<Profile> Profiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
