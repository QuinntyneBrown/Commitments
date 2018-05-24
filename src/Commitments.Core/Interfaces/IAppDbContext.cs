using Commitments.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commitments.Core.Interfaces
{
    public interface IAppDbContext: IDisposable
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<Behaviour> Behaviours { get; set; }
        DbSet<BehaviourType> BehaviourTypes { get; set; }
        DbSet<Card> Cards { get; set; }        
        DbSet<Commitment> Commitments { get; set; }
        DbSet<Dashboard> Dashboards { get; set; }
        DbSet<DashboardCard> DashboardCards { get; set; }
        DbSet<DigitalAsset> DigitalAssets { get; set; }
        DbSet<Frequency> Frequencies { get; set; }
        DbSet<FrequencyType> FrequencyTypes { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<Commitment> ProfileCommitments { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<ToDo> ToDos { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
