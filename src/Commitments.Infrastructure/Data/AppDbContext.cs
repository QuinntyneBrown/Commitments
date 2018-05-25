using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commitments.Infrastructure.Data
{    
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options) { }

        public static readonly LoggerFactory ConsoleLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name 
                && level == LogLevel.Information, true) });

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Behaviour> Behaviours { get; set; }
        public DbSet<BehaviourType> BehaviourTypes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Commitment> Commitments { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<DashboardCard> DashboardCards { get; set; }
        public DbSet<DigitalAsset> DigitalAssets { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<FrequencyType> FrequencyTypes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Commitment> ProfileCommitments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {            
            ChangeTracker.DetectChanges();

            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.Entity is ILoggable && ((e.State == EntityState.Added || (e.State == EntityState.Modified))))
                .Select(x => x.Entity as ILoggable))
            {
                var isNew = entity.CreatedOn == default(DateTime);
                entity.CreatedOn = isNew ? DateTime.UtcNow : entity.CreatedOn;   
                entity.LastModifiedOn = DateTime.UtcNow;
            }

            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
            {
                item.State = EntityState.Modified;
                item.CurrentValues["IsDeleted"] = true;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Behaviour>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<BehaviourType>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Card>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Commitment>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Dashboard>()
                .HasQueryFilter(e => !e.IsDeleted);
            
            modelBuilder.Entity<DashboardCard>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Frequency>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<FrequencyType>()
                .HasQueryFilter(e => !e.IsDeleted);
            
            modelBuilder.Entity<Note>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<NoteTag>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Profile>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Tag>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<ToDo>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<User>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId);

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId);

            modelBuilder.Entity<CommitmentFrequency>()
                .HasOne(nt => nt.Commitment)
                .WithMany(n => n.CommitmentFrequencies)
                .HasForeignKey(nt => nt.CommitmentId);

            modelBuilder.Entity<CommitmentFrequency>()
                .HasOne(nt => nt.Frequency)
                .WithMany(t => t.CommitmentFrequencies)
                .HasForeignKey(nt => nt.FrequencyId);

            base.OnModelCreating(modelBuilder);
        }       
    }
}