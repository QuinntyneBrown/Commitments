using Commitments.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commitments.Core.Interfaces
{
    public interface IAppDbContext: IDisposable
    {
        DbSet<Behaviour> Behaviours { get; set; }
        DbSet<BehaviourType> BehaviourTypes { get; set; }
        DbSet<Commitment> Commitments { get; set; }
        DbSet<CommitmentFrequency> CommitmentFrequencies { get; set; }
        DbSet<DigitalAsset> DigitalAssets { get; set; }
        DbSet<Frequency> Frequencies { get; set; }
        DbSet<FrequencyType> FrequencyTypes { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<Commitment> ProfileCommitments { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
