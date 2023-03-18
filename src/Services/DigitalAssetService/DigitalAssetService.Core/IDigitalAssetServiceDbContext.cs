// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DigitalAssetService.Core;
using Microsoft.EntityFrameworkCore;
using DigitalAssetService.Core.AggregateModel.DigitalAssetAggregate;

namespace DigitalAssetService.Core;

public interface IDigitalAssetServiceDbContext
{
    DbSet<DigitalAsset> DigitalAssets { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}