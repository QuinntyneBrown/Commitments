// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace Commitments.Shared;

public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options)
        : base(options)
    {
        SavingChanges += OnSavingChanges;
    }

    private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
    {
        foreach (var entity in ChangeTracker.Entries()
            .Where(e => e.Entity is ILoggable && (e.State == EntityState.Added || e.State == EntityState.Modified))
            .Select(x => x.Entity as ILoggable))
        {
            var isNew = entity!.CreatedOn == default;
            entity.CreatedOn = isNew ? DateTime.UtcNow : entity.CreatedOn;
            entity.LastModifiedOn = DateTime.UtcNow;
        }

        foreach (var item in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is ILoggable))
        {
            item.State = EntityState.Modified;
            item.CurrentValues["IsDeleted"] = true;
        }
    }
}
