// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using System;


namespace Commitments.Api.Features.DigitalAssets;

public class DigitalAssetDto
{        
    public Guid DigitalAssetId { get; set; }
    public string Name { get; set; }
    public string RelativePath { get { return $"api/digitalassets/serve/{DigitalAssetId}"; } }
    public byte[] Bytes { get; set; }
    public string ContentType { get; set; }
    public static DigitalAssetDto FromDigitalAsset(DigitalAsset digitalAsset)
        => new DigitalAssetDto
        {
            DigitalAssetId = digitalAsset.DigitalAssetId,
            Name = digitalAsset.Name,
            Bytes = digitalAsset.Bytes,
            ContentType = digitalAsset.ContentType
        };
}

