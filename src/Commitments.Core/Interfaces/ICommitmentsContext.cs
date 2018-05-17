using Commitments.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Commitments.Core.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Commitment> Commitments { get; set; }
        DbSet<DigitalAsset> DigitalAssets { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
