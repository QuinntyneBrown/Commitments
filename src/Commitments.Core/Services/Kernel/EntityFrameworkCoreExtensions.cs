// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Commitments.Core.Services.Kernel;

public static class IQueryableExtensions
{
    public static IQueryable<T> IncludeOptimized<T, TProperty>(this IQueryable<T> queryable, System.Linq.Expressions.Expression<Func<T, TProperty>> navigationPropertyPath) where T : class
    {
        return queryable.AsNoTracking().Include(navigationPropertyPath).AsTracking();
    }

    public static void RemoveRangeOptimized(this DbSet<EntityEntry> set, IEnumerable<EntityEntry> entries)
    {
        foreach (var entry in entries)
        {
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
        }
    }
}
