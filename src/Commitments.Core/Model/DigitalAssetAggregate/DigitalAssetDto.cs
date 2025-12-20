// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Model.DigitalAssetAggregate;

public class DigitalAssetDto
{
    public Guid DigitalAssetId { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }

    public static DigitalAssetDto FromDigitalAsset(DigitalAsset entity)
        => new()
        {
            DigitalAssetId = entity.DigitalAssetId,
            Name = entity.Name,
            ContentType = entity.ContentType
        };
}
