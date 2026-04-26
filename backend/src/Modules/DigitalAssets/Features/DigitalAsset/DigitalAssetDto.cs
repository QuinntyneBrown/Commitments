namespace DigitalAssets.Features.DigitalAsset;

public class DigitalAssetDto
{
    public Guid DigitalAssetId { get; set; }
    public byte[]? Bytes { get; set; }
    public string? ContentType { get; set; }
    public string? Name { get; set; }
}
