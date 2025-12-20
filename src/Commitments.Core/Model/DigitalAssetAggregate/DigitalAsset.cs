// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Commitments.Core.Model.DigitalAssetAggregate;

public class DigitalAsset
{
    public Guid DigitalAssetId { get; set; }
    public byte[]? Bytes { get; set; }
    public string? ContentType { get; set; }
    public string? Name { get; set; }
}
