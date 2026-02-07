namespace DigitalAssets.Core.Model.DigitalAssetAggregate;

public class DigitalAsset
{
    public Guid DigitalAssetId { get; set; }
    public byte[]? Bytes { get; set; }
    public string? ContentType { get; set; }
    public string? Name { get; set; }
}
