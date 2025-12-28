// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DigitalAssetAggregate;
using DigitalAssetEntity = Commitments.Core.Model.DigitalAssetAggregate.DigitalAsset;

namespace Commitments.Api.Features.DigitalAsset;

public class DigitalAssetDto
{
    public Guid DigitalAssetId { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }

    public static DigitalAssetDto FromDigitalAsset(DigitalAssetEntity entity)
        => new()
        {
            DigitalAssetId = entity.DigitalAssetId,
            Name = entity.Name,
            ContentType = entity.ContentType
        };
}
